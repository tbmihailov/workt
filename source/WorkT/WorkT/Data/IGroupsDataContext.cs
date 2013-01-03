using System.Collections.Generic;
using WorkT.Model;

namespace WorkT.Data
{
    public interface IGroupsDataContext
    {
        /// <summary>
        /// Inserts a group in the database
        /// </summary>
        /// <param name="group"></param>
        void InsertGroup(Group group);

        /// <summary>
        /// Inserts a group in the database
        /// </summary>
        /// <param name="group"></param>
        void InsertGroupAndSync(Group group);

        /// <summary>
        /// Update group in the database
        /// </summary>
        /// <param name="group"></param>
        void UpdateGroup(Group group);

        void InsertOrUpdateGroup(Group group);

        /// <summary>
        /// Delete group from the database
        /// </summary>
        /// <param name="group"></param>
        void DeleteGroup(Group group);

        /// <summary>
        /// Delete group from database by id
        /// </summary>
        /// <param name="groupId"></param>
        void DeleteGroupById(int groupId);

        /// <summary>
        /// Returns all groups from the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<Group> GetAllGroups();

        /// <summary>
        /// Returns single group from the database by id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>Group</returns>
        Group GetGroupById(int groupId);

        /// <summary>
        /// Creats the Groups table if not exists in the database
        /// </summary>
        void CreateGroupsTableIfNotExists();

        /// <summary>
        /// Drops the groups table if exists in the database
        /// </summary>
        void DropGroupsTableIfExists();
    }
}