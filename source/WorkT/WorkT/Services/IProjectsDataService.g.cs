using System;
using WorkT.Data;
using WorkT.Model;
using System.Collections.Generic;

namespace WorkT.Services
{
	public interface IProjectsDataService
	{
		void Save(Action<OperationResult> callBack, WorkT.Model.Project project);
		void Delete(Action<OperationResult> callBack, WorkT.Model.Project project);
		void GetAllProjects(Action<OperationResult<IEnumerable<Project>>> callBack);
        void GetProjectById(Action<OperationResult<Project>> callBack, int projectId);
	}
}


