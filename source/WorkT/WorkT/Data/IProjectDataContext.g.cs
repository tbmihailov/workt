using System.Collections.Generic;
using WorkT.Model;

namespace WorkT.Data
{
	public interface IProjectDataContext
	{
		/// <summary>
		/// Inserts a project in the database
		/// </summary>
		/// <param name="project"></param>
		void InsertProject(Project project);

		/// <summary>
		/// Inserts a project in the database
		/// </summary>
		/// <param name="project"></param>
		void InsertProjectAndSync(Project project);

		/// <summary>
		/// Update project in the database
		/// </summary>
		/// <param name="project"></param>
		void UpdateProject(Project project);

		void InsertOrUpdateProject(Project project);

		/// <summary>
		/// Delete project from the database
		/// </summary>
		/// <param name="project"></param>
		void DeleteProject(Project project);

		/// <summary>
		/// Delete project from database by id
		/// </summary>
		/// <param name="projectId"></param>
		void DeleteProjectById(int projectId);

		/// <summary>
		/// Returns all projects from the database
		/// </summary>
		/// <returns></returns>
		IEnumerable<Project> GetAllProjects();

		/// <summary>
		/// Returns single project from the database by id
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns>Project</returns>
		Project GetProjectById(int projectId);

		/// <summary>
		/// Creats the Projects table if not exists in the database
		/// </summary>
		void CreateProjectsTableIfNotExists();

		/// <summary>
		/// Drops the projects table if exists in the database
		/// </summary>
		void DropProjectsTableIfExists();
	}
}
