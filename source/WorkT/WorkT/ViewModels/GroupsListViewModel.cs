using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using WorkT.Model;
using WorkT.Services;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using WorkT.Messages;

namespace WorkT.ViewModels
{
    public class GroupsListViewModel : ViewModelBase
    {
        IGroupsDataService _groupsDataService = new GroupsDataService();

        public GroupsListViewModel()
        {
            this.Initialize();

            this.AddSampleGroupsToDb();
            this.Load();
        }

        private void Initialize()
        {
            this._groupsDataService = ServiceProvider.Instance.GroupsDataService;
            this._groups = new ObservableCollection<GroupViewModel>();
        }

        private string isBusyPropertyName = "IsBusy";
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

        #region Groups list property
        public const string GroupsPropertyName = "Groups";
        private ObservableCollection<GroupViewModel> _groups;
        public ObservableCollection<GroupViewModel> Groups
        {
            get
            {
                return _groups;
            }

            set
            {
                if (_groups == value)
                {
                    return;
                }

                var oldValue = _groups;;
                _groups = value;

                RaisePropertyChanged(GroupsPropertyName);
            }
        }
        #endregion

        public void Load()
        {
            IsBusy = true;
            Groups.Clear();
            _groupsDataService.GetAllGroups(OnGroupsLoaded);
        }

        private void OnGroupsLoaded(OperationResult<IEnumerable<Group>> result)
        {
            IsBusy = false;
            if (result.HasError)
            {
                var error = result.Error;
                MessageBox.Show(string.Format("Възникна грешка при зареждане на групите! \n{0}", error.Message));
                return;
            }

            var groups = result.Item;
            Groups = new ObservableCollection<GroupViewModel>();

            foreach (var group in groups)
            {
                GroupViewModel groupViewModel = new GroupViewModel(group);
                Groups.Add(groupViewModel);
            }
        }

        public void LoadDesignGroups()
        {
            var groups = new List<GroupViewModel>();
            for (int i = 0; i < 10; i++)
            {
                var group = new Group()
                {
                    GroupId = i,
                    Name = string.Format("Name {0}", i),
                    Description = string.Format("Description of group {0}", i)
                };

                var groupViewModel = new GroupViewModel(group);
                groups.Add(groupViewModel);
            }

            Groups = new ObservableCollection<GroupViewModel>();
            foreach (var group in groups)
            {
                Groups.Add(group);
            }
        }


        public override void Cleanup()
        {
            base.Cleanup();
        }

        #region Test data
        public void AddSampleGroupsToDb()
        {
            int groupsCount = 10;
            var groups = new List<Group>();
            for (int i = 0; i < groupsCount; i++)
            {
                var group = new Group()
                {
                    GroupId = 0,
                    Name = string.Format("Name {0}", i),
                    Description = string.Format("Description of group {0}", i)
                };

                groups.Add(group);
            }

            foreach (Group gr in groups)
            {
                _groupsDataService.Save(OnGroupSaved, gr);
            }
        }

        private void OnGroupSaved(OperationResult result)
        {
        }
        #endregion

        #region AddCommand
        private RelayCommand _doSomethingCommand;
        public RelayCommand AddCommand
        {
            get
            {
                if (_doSomethingCommand == null)
                {
                    _doSomethingCommand = new RelayCommand(
                            () =>
                            {
                                AddExecute();
                            },
                            () => CanAdd
                        );
                }
                return _doSomethingCommand;
            }
            set
            {
                _doSomethingCommand = value;
            }
        }
        public void AddExecute()
        {
            this.NavigateToGroupAddView();
        }

        private void NavigateToGroupAddView()
        {
            //PageConductor.Instance.NavigateToGroupAddView();
            var addNewGroupMessage = new AddNewGroupMessage();
            Messenger.Default.Send(addNewGroupMessage);
        }

        public const string CanAddPropertyName = "CanAdd";
        private bool _canAdd = true;
        public bool CanAdd
        {
            get
            {
                return _canAdd;
            }

            set
            {
                if (_canAdd == value)
                {
                    return;
                }

                var oldValue = _canAdd;
                _canAdd = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanAddPropertyName);

                //tells the controls that are binded to the Command if it can execute
                AddCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region LoadCommand
        private RelayCommand _loadCommand;
        public RelayCommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new RelayCommand(
                            () =>
                            {
                                LoadExecute();
                            },
                            () => CanLoad
                        );
                }
                return _loadCommand;
            }
            set
            {
                _loadCommand = value;
            }
        }

        public void LoadExecute()
        {
            this.Load();
        }

        public const string CanLoadPropertyName = "CanLoad";
        private bool _canLoad = true;
        public bool CanLoad
        {
            get
            {
                return _canLoad;
            }

            set
            {
                if (_canLoad == value)
                {
                    return;
                }

                var oldValue = _canLoad;
                _canLoad = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanLoadPropertyName);

                //tells the controls that are binded to the Command if it can execute
                LoadCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion



    }
}