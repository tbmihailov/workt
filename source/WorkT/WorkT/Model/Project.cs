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

namespace WorkT.Model
{
    public class Project
    {
        private int _projectId;
        public int ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        private int _groupId;
        public int GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _descripton;
        public string Description
        {
            get { return _descripton; }
            set { _descripton = value; }
        }

    }
}
