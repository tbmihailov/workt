using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using WorkT.Messages;

namespace WorkT.Services
{
    public class PageConductor
    {
        private static PageConductor _instance;
        public static PageConductor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PageConductor();
                }
                return _instance;
            }
        }

        public PageConductor()
        {
            this.RegisterMessages();
        }

        public void RegisterMessages()
        {
            this.RegisterCommonNavigationMessages();
            this.RegisterGroupMessages();
        }

        #region Common navigation
        private void RegisterCommonNavigationMessages()
        {
            Messenger.Default.Register<GoBackMessage>(this, OnGoBackMessage);
            Messenger.Default.Register<GoForwardMessage>(this, OnGoForwardMessage);
        }

        public void OnGoBackMessage(GoBackMessage message)
        {
            NavigationController.Instance.GoBack();
        }

        public void OnGoForwardMessage(GoForwardMessage message)
        {
            NavigationController.Instance.GoForward();
        }
        #endregion

        #region Groups navigation
        private void RegisterGroupMessages()
        {
            Messenger.Default.Register<AddNewGroupMessage>(this, OnAddNewGroupMessage);
        }

        public void OnAddNewGroupMessage(AddNewGroupMessage message)
        {
            this.NavigateToGroupAddView();
        }

        public void NavigateToGroupsView()
        {
            var navController = NavigationController.Instance;
            var address = "/Groups";
            var uri = new Uri(address, UriKind.Relative);
            navController.Navigate(uri);
        }

        public void NavigateToGroupAddView()
        {
            var navController = NavigationController.Instance;
            var address = "/Groups/Add";
            var uri = new Uri(address, UriKind.Relative);
            navController.Navigate(uri);
        }

        public void NavigateToGroupEditView()
        {
            var navController = NavigationController.Instance;
            var address = "/Groups/Edit";
            var uri = new Uri("/Groups/Edit", UriKind.Relative);
            navController.Navigate(uri);
        }
        #endregion
    }
}
