using System;
using System.Linq;
using System.Collections.Generic;
using WorkT.Model;
using SQLiteClient;

namespace WorkT.Data
{
    public class GroupsDataContext : IGroupsDataContext
    {

        public GroupsDataContext() { }

        private const string SQL_GROUP_DELETE_BY_ID = "DELETE FROM Groups where GroupId = ?";
        private const string SQL_GROUP_INSERT = "INSERT INTO Groups(Name, Description) VALUES(?,?)";
        private const string SQL_SELECT_LAST_INSERT_ID = "SELECT last_insert_rowid() as Id";
        private const string SQL_GROUP_UPDATE = "UPDATE Groups SET " +
                                                      "Name = ?," +
                                                      "Description=?" +
                                                      "where GroupId=?";
        private const string SQL_GROUP_SELECT_ALL = "SELECT * FROM Groups";
        private const string SQL_GROUP_SELECT_BY_ID = "SELECT * FROM Groups where GroupId = ?";
        private const string SQL_GROUP_TABLE_CREATE = "CREATE TABLE IF NOT EXISTS Groups " +
                                                        "(" +
                                                            "GroupId INTEGER PRIMARY KEY  NOT NULL ," +
                                                            "Name VARCHAR(32)," +
                                                            "Description varchar(160)" +
                                                         ")";
        private const string SQL_GROUP_TABLE_DROP = "DROP TABLE IF EXISTS Groups";
    
        public GroupsDataContext(SQLiteConnection sqlConnection)
            : base()
        {
            this.db = sqlConnection;
        }

        private SQLiteConnection db;

        /// <summary>
        /// Inserts a group in the database
        /// </summary>
        /// <param name="group"></param>
        public void InsertGroup(Group group)
        {
            var currentDatabase = db;

            if (group == null)
            {
                throw new ArgumentNullException("group must not be null");
            }

            if (currentDatabase == null)
            {
                return;
            }
            try
            {
                DateTime start = DateTime.Now;
                currentDatabase.BeginTransaction();
                SQLiteCommand cmd = currentDatabase.CreateCommand(SQL_GROUP_INSERT,
                                                     group.Name, group.Description);
                int numberOfRowsAffected = cmd.ExecuteNonQuery();
                currentDatabase.CommitTransaction();
            }
            catch (SQLiteException ex)
            {
                if (currentDatabase.TransactionOpened)
                    currentDatabase.RollbackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// Inserts a group in the database
        /// </summary>
        /// <param name="group"></param>
        public void InsertGroupAndSync(Group group)
        {
            var currentDatabase = db;

            if (group == null)
            {
                throw new ArgumentNullException("group must not be null");
            }

            if (currentDatabase == null)
            {
                return;
            }
            try
            {
                DateTime start = DateTime.Now;
                currentDatabase.BeginTransaction();
                //save new group
                SQLiteCommand cmd = currentDatabase.CreateCommand(SQL_GROUP_INSERT,
                                                     group.Name, group.Description);
                int numberOfRowsAffected = cmd.ExecuteNonQuery();
                
                //load new inserted group
                SQLiteCommand cmdLoad = currentDatabase.CreateCommand(SQL_SELECT_LAST_INSERT_ID);
                var lastRowIdResult = cmdLoad.ExecuteQuery<ScalarIdColumn>();
                var lastRowId = lastRowIdResult.FirstOrDefault();

                int lastId = lastRowId.Id;

                //set new values
                group.GroupId = lastId;
                currentDatabase.CommitTransaction();
            }
            catch (SQLiteException ex)
            {
                if (currentDatabase.TransactionOpened)
                    currentDatabase.RollbackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// Update group in the database
        /// </summary>
        /// <param name="group"></param>
        public void UpdateGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group must not be null");
            }

            if (db == null)
            {
                return;
            }
            try
            {
                db.BeginTransaction();
                SQLiteCommand cmd = db.CreateCommand(SQL_GROUP_UPDATE,
                                                      group.Name,
                                                      group.Description,
                                                      group.GroupId);
                int numberOfRowsAffected = cmd.ExecuteNonQuery();
                db.CommitTransaction();
            }
            catch (SQLiteException ex)
            {
                if (db.TransactionOpened)
                    db.RollbackTransaction();
                throw ex;
            }
        }

        public void InsertOrUpdateGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group must not be null");
            }

            int groupId = group.GroupId;
            if (groupId == 0)
            {
                this.InsertGroupAndSync(group);
            }
            else
            {
                this.UpdateGroup(group);
            }
        }

        /// <summary>
        /// Delete group from the database
        /// </summary>
        /// <param name="group"></param>
        public void DeleteGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group must not be null");
            }

            int groupId = group.GroupId;

            this.DeleteGroupById(groupId);
        }

        /// <summary>
        /// Delete group from database by id
        /// </summary>
        /// <param name="groupId"></param>
        public void DeleteGroupById(int groupId)
        {
            if (db == null)
            {
                return;
            }
            try
            {
                db.BeginTransaction();
                SQLiteCommand cmd = db.CreateCommand(SQL_GROUP_DELETE_BY_ID,
                                                      groupId);
                int numberOfRowsAffected = cmd.ExecuteNonQuery();
                db.CommitTransaction();
            }
            catch (SQLiteException ex)
            {
                if (db.TransactionOpened)
                    db.RollbackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// Returns all groups from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Group> GetAllGroups()
        {
            if (db == null)
            {
                return null;
            }

            try
            {
                SQLiteCommand cmd = db.CreateCommand(SQL_GROUP_SELECT_ALL);
                var groups = cmd.ExecuteQuery<Group>();
                return groups;
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns single group from the database by id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>Group</returns>
        public Group GetGroupById(int groupId)
        {
            if (db == null)
            {
                return null;
            }

            try
            {
                SQLiteCommand cmd = db.CreateCommand(SQL_GROUP_SELECT_BY_ID,
                                                        groupId);
                var groups = cmd.ExecuteQuery<Group>();
                var group = groups.FirstOrDefault();
                return group;
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creats the Groups table if not exists in the database
        /// </summary>
        public void CreateGroupsTableIfNotExists()
        {
            if (db == null)
            {
                return;
            }
            try
            {
                db.BeginTransaction();
                SQLiteCommand cmd = db.CreateCommand(
                            SQL_GROUP_TABLE_CREATE);
                int numberOfRowsAffected = cmd.ExecuteNonQuery();
                db.CommitTransaction();
            }
            catch (SQLiteException ex)
            {
                if (db.TransactionOpened)
                    db.RollbackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// Drops the groups table if exists in the database
        /// </summary>
        public void DropGroupsTableIfExists()
        {
            if (db == null)
            {
                return;
            }
            try
            {
                db.BeginTransaction();
                SQLiteCommand cmd = db.CreateCommand(
                            SQL_GROUP_TABLE_DROP);
                int numberOfRowsAffected = cmd.ExecuteNonQuery();
                db.CommitTransaction();
            }
            catch (SQLiteException ex)
            {
                if (db.TransactionOpened)
                    db.RollbackTransaction();
                throw ex;
            }
        }

    }
}
