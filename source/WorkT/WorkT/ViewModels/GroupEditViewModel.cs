using GalaSoft.MvvmLight;
using WorkT.Services;
using WorkT.Model;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using WorkT.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace WorkT.ViewModels
{
    public class GroupEditViewModel : ViewModelBase
    {
        IGroupsDataService _groupsDataService;

        public GroupEditViewModel()
        {
            _groupsDataService = ServiceProvider.Instance.GroupsDataService;

            if (IsInDesignModeStatic)
            {
                this.Load(1);
            }
            else
            {
                this.CreateNew();
            }
        }

        public GroupEditViewModel(int groupId)
        {
            if (groupId == 0)
            {
                this.CreateNew();
            }
            else
            {
                this.Load(groupId);
            }
        }


        private const string TitlePropertyName = "Title";
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }
                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        private void UpdateTitle()
        {
            if (Group == null)
            {
                Title = "Edit group";
                return;
            }

            int groupId = Group.GroupId;
            if (groupId == 0)
            {
                Title = "New group";
            }
            else
            {
                Title = "Edit group";
            }

        }

        public void CreateNew()
        {
            var newGroup = new Group();
            this.Group = new GroupViewModel(newGroup);
            UpdateTitle();
        }

        public void Load(int groupId)
        {
            _groupsDataService.GetGroupById(OnGroupLoaded, groupId);
        }

        private void OnGroupLoaded(OperationResult<Group> result)
        {
            if (result.HasError)
            {
                var error = result.Error;
                DisplayMessage(error.Message, "Load error");
            }
            var group = result.Item;
            this.Group = new GroupViewModel(group);
            UpdateTitle();
        }

        private readonly string isBusyPropertyName = "IsBusy";
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy == value)
                {
                    return;
                }
                _isBusy = value;
                RaisePropertyChanged(isBusyPropertyName);
            }
        }

        public const string GroupPropertyName = "Group";
        private GroupViewModel _group;
        public GroupViewModel Group
        {
            get
            {
                return _group;
            }

            set
            {
                if (_group == value)
                {
                    return;
                }

                var oldValue = _group;
                _group = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(GroupPropertyName);
            }
        }

        #region SaveCommand

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                            () =>
                            {
                                SaveExecute();
                            },
                            () => CanSave
                        );
                }
                return _saveCommand;
            }
            set
            {
                _saveCommand = value;
            }
        }

        public void SaveExecute()
        {
            bool isDataValid = true;

            ValidateData(out isDataValid);
            if (!isDataValid)
            {
                return;
            }

            var group = this.Group.Group;
            IsBusy = true;
            _groupsDataService.Save(OnGroupSaved, group);
        }

        private void ValidateData(out bool isDataValid)
        {
            string errorMessage = string.Empty;
            bool hasError = false;
            isDataValid = true;

            bool isGroupNameValid = IsGroupNameValid();
            if (!isGroupNameValid)
            {
                errorMessage += "Name is empty or invalid!\n";
                hasError = true;
            }

            if (hasError)
            {
                DisplayMessage(errorMessage, "Data error");
            }

            isDataValid = !hasError;

            return;
        }

        private void DisplayMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }


        private bool IsDataValid()
        {
            if (IsGroupNameValid())
            {
                return false;
            }

            return true;
        }

        private bool IsGroupNameValid()
        {
            bool isGroupNameValid = !((string.IsNullOrEmpty(Group.Name)
                                    || string.IsNullOrWhiteSpace(Group.Name)));
            return isGroupNameValid;
        }

        private void OnGroupSaved(OperationResult result)
        {
            IsBusy = false;
            if (result.HasError)
            {
                var error = result.Error;
                DisplayMessage(string.Format("Save unsuccessfull!\n {0}", error.Message), "Save");
                return;
            }

            DisplayMessage(string.Format("Save successfull!"), "Save");
            this.RaisePropertyChanged(GroupPropertyName);
            this.UpdateTitle();
        }

        public const string CanSavePropertyName = "CanSave";
        private bool _canSave = true;
        public bool CanSave
        {
            get
            {
                return _canSave;
            }

            set
            {
                if (_canSave == value)
                {
                    return;
                }

                var oldValue = _canSave;
                _canSave = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanSavePropertyName);

                //tells the controls that are binded to the Command if it can execute
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region CancelCommand

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(
                            () =>
                            {
                                CancelExecute();
                            },
                            () => CanCancel
                        );
                }
                return _cancelCommand;
            }
            set
            {
                _cancelCommand = value;
            }
        }

        public void CancelExecute()
        {
            GoBack();
        }

        private static void GoBack()
        {
            SendGoBackMessage();
        }

        private static void SendGoBackMessage()
        {
            var goBackMessage = new GoBackMessage();
            Messenger.Default.Send(goBackMessage);
        }

        public const string CanCancelPropertyName = "CanCancel";
        private bool _canCancel = true;
        public bool CanCancel
        {
            get
            {
                return _canCancel;
            }

            set
            {
                if (_canCancel == value)
                {
                    return;
                }

                var oldValue = _canCancel;
                _canCancel = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanCancelPropertyName);

                //tells the controls that are binded to the Command if it can execute
                CancelCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}