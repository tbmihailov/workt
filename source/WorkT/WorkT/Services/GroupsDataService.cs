using System;
using WorkT.Data;
using WorkT.Model;
using System.Collections.Generic;

namespace WorkT.Services
{
    public class GroupsDataService : WorkT.Services.IGroupsDataService
    {
        WorkTrackerDataContext _context;

        public GroupsDataService()
        {

        }

        public GroupsDataService(string databaseFileName)
            : this()
        {
            _context = new WorkTrackerDataContext(databaseFileName);
        }

        public void Save(Action<OperationResult> callBack, Model.Group group)
        {
            try
            {
                if (group == null)
                {
                    throw new ArgumentNullException("group must not be null!");
                }

                _context.InsertOrUpdateGroup(group);

                var successfullResult = new OperationResult();
                callBack(successfullResult);
            }
            catch (Exception e)
            {
                var errorResult = new OperationResult(e);
                callBack(errorResult);
            }

        }

        public void Delete(Action<OperationResult> callBack, Model.Group group)
        {
            try
            {
                if (group == null)
                {
                    throw new ArgumentNullException("group must not be null!");
                }

                _context.DeleteGroup(group);

                var successfullResult = new OperationResult();
                callBack(successfullResult);
            }
            catch (Exception e)
            {
                var errorResult = new OperationResult(e);
                callBack(errorResult);
            }
        }

        public void GetAllGroups(Action<OperationResult<IEnumerable<Group>>> callBack)
        {
            try
            {
                var groups = _context.GetAllGroups();

                var successfullResult = new OperationResult<IEnumerable<Group>>(groups);
                callBack(successfullResult);
            }
            catch (Exception e)
            {
                var errorResult = new OperationResult<IEnumerable<Group>>(e);
                callBack(errorResult);
            }
        }

        public void GetGroupById(Action<OperationResult<Group>> callBack, int groupId)
        {
            try
            {
                if (groupId == 0)
                {
                    throw new ArgumentNullException("groupId must be greater than 0!");
                }

                var groups = _context.GetGroupById(groupId);

                var successfullResult = new OperationResult<Group>(groups);
                callBack(successfullResult);
            }
            catch (Exception e)
            {
                var errorResult = new OperationResult<Group>(e);
                callBack(errorResult);
            }
        }
    }
}
