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

namespace WorkT.CodeGeneration.Model
{
    public class Group
    {
        public Group() { }

        public Group(string name, string descritpion)
            : base()
        {
            this.Name = name;
            this.Description = descritpion;
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

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

    }
}
