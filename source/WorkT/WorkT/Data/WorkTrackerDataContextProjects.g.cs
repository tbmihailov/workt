using System;
using System.Linq;
using System.Collections.Generic;
using WorkT.Model;
using SQLiteClient;

namespace WorkT.Data
{
	public partial class WorkTrackerDataContext 
	{
		//TO DO: Uncomment these region with common datacontext operations in one of the datacontexts
		#region Common DataContext
		//private SQLiteConnection _db;
		//private string _databaseFileName;
		//private string DatabaseFileName
		//{
		//    get { return _databaseFileName; }
		//    set { _databaseFileName = value; }
		//}

		//public WorkTrackerDataContext ()
		//{
		//}

		//public WorkTrackerDataContext (string fileName)
		//    : this()
		//{
		//    this._databaseFileName = fileName;
		//
		//    _db = new SQLiteConnection(fileName);
		//
		//    this.Open();
		//    this.CreateTablesIfNotExist();
		//}

		//private void CreateTablesIfNotExist()
		//{
		//	//TO DO: Add table creation methods here
		//    this.CreateProjectsTableIfNotExists();
		//}

		///// <summary>
		///// Opens the database
		///// </summary>
		//public void Open()
		//{
		//    if (_db == null)
		//    {
		//        _db = new SQLiteConnection(DatabaseFileName);
		//    }

		//    try
		//    {
		//        _db.Open();
		//    }
		//    catch (SQLiteException ex)
		//    {
		//        throw ex;
		//    }
		//}
		#endregion


		#region Projects Operations
		private ProjectDataContext _projectDataContext;
		public ProjectDataContext ProjectDataContext
		{
			get
			{
				if (_projectDataContext == null)
				{
					_projectDataContext = new ProjectDataContext(_db);
				}
				return _projectDataContext;
			}
			set
			{
				_projectDataContext = value;
			}
		}

		/// <summary>
		/// Inserts a project in the database
		/// </summary>
		/// <param name="project"></param>
		public void InsertProject(Project project)
		{
			ProjectDataContext.InsertProject(project);
		}

		/// <summary>
		/// Inserts a project in the database
		/// </summary>
		/// <param name="project"></param>
		public void InsertProjectAndSync(Project project)
		{
			ProjectDataContext.InsertProjectAndSync(project);
		}

		/// <summary>
		/// Update project in the database
		/// </summary>
		/// <param name="project"></param>
		public void UpdateProject(Project project)
		{
			ProjectDataContext.UpdateProject(project);
		}

		public void InsertOrUpdateProject(Project project)
		{
			ProjectDataContext.InsertOrUpdateProject(project);
		}

		/// <summary>
		/// Delete project from the database
		/// </summary>
		/// <param name="project"></param>
		public void DeleteProject(Project project)
		{
			ProjectDataContext.DeleteProject(project);
		}

		/// <summary>
		/// Delete project from database by id
		/// </summary>
		/// <param name="projectId"></param>
		public void DeleteProjectById(int projectId)
		{
			ProjectDataContext.DeleteProjectById(projectId);
		}

		/// <summary>
		/// Returns all projects from the database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Project> GetAllProjects()
		{
			return ProjectDataContext.GetAllProjects();
		}

		/// <summary>
		/// Returns single project from the database by id
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns>Project</returns>
		public Project GetProjectById(int projectId)
		{
			return ProjectDataContext.GetProjectById(projectId);
		}

		/// <summary>
		/// Creates the Projects table if not exists in the database
		/// </summary>
		public void CreateProjectsTableIfNotExists()
		{
			ProjectDataContext.CreateProjectsTableIfNotExists();
		}

		/// <summary>
		/// Drops the projects table if exists in the database
		/// </summary>
		public void DropProjectsTableIfExists()
		{
			ProjectDataContext.DropProjectsTableIfExists();
		}
		#endregion
	}
}


