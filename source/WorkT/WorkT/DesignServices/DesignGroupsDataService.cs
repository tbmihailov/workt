using System;
using WorkT.Services;
using System.Collections;
using WorkT.Model;
using System.Collections.Generic;
using WorkT.DesignModel;

namespace WorkT.DesignServices
{
    public class DesignGroupsDataService : IGroupsDataService
    {

        public void Delete(Action<OperationResult> callBack, Model.Group group)
        {
            var result = new OperationResult();
            callBack(result);
        }

        public void GetAllGroups(Action<OperationResult<System.Collections.Generic.IEnumerable<Model.Group>>> callBack)
        {
            IEnumerable<Group> groups = new DesignGroups();
            var result = new OperationResult<System.Collections.Generic.IEnumerable<Model.Group>>(groups);
            callBack(result);
        }

        public void GetGroupById(Action<OperationResult<Model.Group>> callBack, int groupId)
        {
            Group group = new Group("Test group", "Test projects group.");
            group.GroupId = groupId;
            var result = new OperationResult<Model.Group>(group);
            callBack(result);
        }

        public void Save(Action<OperationResult> callBack, Model.Group group)
        {
            var result = new OperationResult();
            callBack(result);
        }
    }
}
