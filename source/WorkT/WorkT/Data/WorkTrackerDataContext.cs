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
using SQLiteClient;

namespace WorkT.Data
{
    public partial class WorkTrackerDataContext
    {
        private SQLiteConnection _db;
        private string _databaseFileName;
        private string DatabaseFileName
        {
            get { return _databaseFileName; }
            set { _databaseFileName = value; }
        }

        public WorkTrackerDataContext()
        {
        }

        public WorkTrackerDataContext(string fileName)
            : this()
        {
            this._databaseFileName = fileName;

            _db = new SQLiteConnection(fileName);

            this.Open();
            this.CreateTablesIfNotExist();
        }

        private void CreateTablesIfNotExist()
        {
            this.CreateGroupsTableIfNotExists();
        }

        /// <summary>
        /// Opens the database
        /// </summary>
        public void Open()
        {
            if (_db == null)
            {
                _db = new SQLiteConnection(DatabaseFileName);
            }

            try
            {
                _db.Open();
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
        }

        #region Groups Operations
        private GroupsDataContext _groupsDataCoontext;
        public GroupsDataContext GroupsDataContext
        {
            get
            {
                if (_groupsDataCoontext == null)
                {
                    _groupsDataCoontext = new GroupsDataContext(_db);
                }
                return _groupsDataCoontext;
            }
            set
            {
                _groupsDataCoontext = value;
            }
        }

        public void CreateGroupsTableIfNotExists()
        {
            GroupsDataContext.CreateGroupsTableIfNotExists();
        }

        public void DeleteGroup(Model.Group group)
        {
            GroupsDataContext.DeleteGroup(group);
        }

        public void DeleteGroupById(int groupId)
        {
            GroupsDataContext.DeleteGroupById(groupId);
        }

        public void DropGroupsTableIfExists()
        {
            GroupsDataContext.DropGroupsTableIfExists();
        }

        public System.Collections.Generic.IEnumerable<Model.Group> GetAllGroups()
        {
            var groups = GroupsDataContext.GetAllGroups();
            return groups;
        }

        public Model.Group GetGroupById(int groupId)
        {
            var group = GroupsDataContext.GetGroupById(groupId);
            return group;
        }

        public void InsertGroup(Model.Group group)
        {
            GroupsDataContext.InsertGroup(group);
        }

        public void InsertOrUpdateGroup(Model.Group group)
        {
            GroupsDataContext.InsertOrUpdateGroup(group);
        }

        public void UpdateGroup(Model.Group group)
        {
            GroupsDataContext.UpdateGroup(group);
        }

        #endregion


    }
}
