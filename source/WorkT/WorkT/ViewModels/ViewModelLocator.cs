/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:WorkT.ViewModels"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using WorkT.Services;
using GalaSoft.MvvmLight.Messaging;
using WorkT.Messages;
namespace WorkT.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            this.RegisterMessages();
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view models
            }
            else
            {
                CreatePageConductor();
            }
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<AddNewGroupMessage>(this, OnAddNewGroupMessage);
        }

        private void OnAddNewGroupMessage(AddNewGroupMessage message)
        {
            this.GroupEditViewModel.CreateNew();
        }


        #region PageConductor
        private static PageConductor _pageConductor;

        /// <summary>
        /// Gets the PageConductor property.
        /// </summary>
        public static PageConductor PageConductorStatic
        {
            get
            {
                if (_pageConductor == null)
                {
                    CreatePageConductor();
                }

                return _pageConductor;
            }
        }

        /// <summary>
        /// Gets the PageConductor property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PageConductor PageConductor
        {
            get
            {
                return PageConductorStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the PageConductor property.
        /// </summary>
        public static void ClearPageConductor()
        {
            _pageConductor = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the PageConductor property.
        /// </summary>
        public static void CreatePageConductor()
        {
            if (_pageConductor == null)
            {
                _pageConductor = PageConductor.Instance;
            }
        }

        #endregion

        #region GroupsListViewModel
        private static GroupsListViewModel _groupsListViewModel;

        /// <summary>
        /// Gets the GroupsListViewModel property.
        /// </summary>
        public static GroupsListViewModel GroupsListViewModelStatic
        {
            get
            {
                if (_groupsListViewModel == null)
                {
                    CreateGroupsListViewModel();
                }

                return _groupsListViewModel;
            }
        }

        /// <summary>
        /// Gets the GroupsListViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public GroupsListViewModel GroupsListViewModel
        {
            get
            {
                return GroupsListViewModelStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the GroupsListViewModel property.
        /// </summary>
        public static void ClearGroupsListViewModel()
        {
            _groupsListViewModel.Cleanup();
            _groupsListViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the GroupsListViewModel property.
        /// </summary>
        public static void CreateGroupsListViewModel()
        {
            if (_groupsListViewModel == null)
            {
                _groupsListViewModel = new GroupsListViewModel();
            }
        }
        #endregion

        #region GroupEditViewModel
        private static GroupEditViewModel _groupEditViewModel;

        /// <summary>
        /// Gets the GroupEditViewModel property.
        /// </summary>
        public static GroupEditViewModel GroupEditViewModelStatic
        {
            get
            {
                if (_groupEditViewModel == null)
                {
                    CreateGroupEditViewModel();
                }

                return _groupEditViewModel;
            }
        }

        /// <summary>
        /// Gets the GroupEditViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public GroupEditViewModel GroupEditViewModel
        {
            get
            {
                return GroupEditViewModelStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the GroupEditViewModel property.
        /// </summary>
        public static void ClearGroupEditViewModel()
        {
            _groupEditViewModel.Cleanup();
            _groupEditViewModel = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the GroupEditViewModel property.
        /// </summary>
        public static void CreateGroupEditViewModel()
        {
            if (_groupEditViewModel == null)
            {
                _groupEditViewModel = new GroupEditViewModel();
            }
        }
        #endregion


        public static void Cleanup()
        {
            ClearPageConductor();
            ClearGroupsListViewModel();
            ClearGroupEditViewModel();
        }
    }
}