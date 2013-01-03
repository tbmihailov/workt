using System;
namespace WorkT.Services
{
    public interface IGroupsDataService
    {
        void Delete(Action<WorkT.OperationResult> callBack, WorkT.Model.Group group);
        void GetAllGroups(Action<WorkT.OperationResult<System.Collections.Generic.IEnumerable<WorkT.Model.Group>>> callBack);
        void GetGroupById(Action<WorkT.OperationResult<WorkT.Model.Group>> callBack, int groupId);
        void Save(Action<WorkT.OperationResult> callBack, WorkT.Model.Group group);
    }
}
