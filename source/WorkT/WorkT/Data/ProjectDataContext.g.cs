using System;
using System.Linq;
using System.Collections.Generic;
using WorkT.Model;
using SQLiteClient;

namespace WorkT.Data
{
	public class ProjectDataContext : IProjectDataContext
	{

		public ProjectDataContext() { }

		private const string SQL_PROJECT_DELETE_BY_ID = "DELETE FROM Projects where ProjectId = ?";
		//private const string SQL_PROJECT_INSERT = "INSERT INTO Projects(Name, Description) VALUES(?,?)";
		private const string SQL_PROJECT_INSERT = "INSERT INTO Projects(GroupId, Name, Description) VALUES(?, ?, ?)";
		private const string SQL_SELECT_LAST_INSERT_ID = "SELECT last_insert_rowid() as Id";
		//private const string SQL_PROJECT_UPDATE = "UPDATE Projects SET " +
		//											  "Name = ?," +
		//											  "Description=?" +
		//											  "where ProjectId=?";
		private const string SQL_PROJECT_UPDATE = 
			@"UPDATE Projects SET 
				GroupId = ?, 
				Name = ?, 
				Description = ? 
			WHERE (ProjectId = ?)";
		private const string SQL_PROJECT_SELECT_ALL = "SELECT * FROM Projects";
		private const string SQL_PROJECT_SELECT_BY_ID = "SELECT * FROM Projects where ProjectId = ?";
		
		//private const string SQL_PROJECT_TABLE_CREATE = "CREATE TABLE IF NOT EXISTS Projects " +
		//												//TODO: Fix table column types
		//												"(" +
		//													"ProjectId INTEGER PRIMARY KEY  NOT NULL ," +
		//													"Name VARCHAR(32)," +
		//													"Description varchar(160)" +
		//												 ")";
		//TO DO: Check Create table script fields Projects
		private const string SQL_PROJECT_TABLE_CREATE =
			@"
			CREATE TABLEProjects
			(
				ProjectId  INTEGER PRIMARY KEY  NOT NULL ,
				GroupId INTEGER, 
				Name VARCHAR(32), 
				Description VARCHAR(32));";

		private const string SQL_PROJECT_TABLE_DROP = "DROP TABLE IF EXISTS Projects";
	
		public ProjectDataContext(SQLiteConnection sqlConnection)
			: base()
		{
			this.db = sqlConnection;
		}

		private SQLiteConnection db;

		/// <summary>
		/// Inserts a project in the database
		/// </summary>
		/// <param name="project"></param>
		public void InsertProject(Project project)
		{
			var currentDatabase = db;

			if (project == null)
			{
				throw new ArgumentNullException("project must not be null");
			}

			if (currentDatabase == null)
			{
				return;
			}
			try
			{
				
				currentDatabase.BeginTransaction();
				SQLiteCommand cmd = currentDatabase.CreateCommand(SQL_PROJECT_INSERT,
													 project.Name, project.Description);
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
		/// Inserts a project in the database
		/// </summary>
		/// <param name="project"></param>
		public void InsertProjectAndSync(Project project)
		{
			var currentDatabase = db;

			if (project == null)
			{
				throw new ArgumentNullException("project must not be null");
			}

			if (currentDatabase == null)
			{
				return;
			}
			try
			{
				
				currentDatabase.BeginTransaction();
				//save new project
				SQLiteCommand cmd = currentDatabase.CreateCommand(SQL_PROJECT_INSERT,
													 project.Name, 
													 project.Description
													 );
				int numberOfRowsAffected = cmd.ExecuteNonQuery();
				
				//load new inserted project
				SQLiteCommand cmdLoad = currentDatabase.CreateCommand(SQL_SELECT_LAST_INSERT_ID);
				var lastRowIdResult = cmdLoad.ExecuteQuery<ScalarIdColumn>();
				var lastRowId = lastRowIdResult.FirstOrDefault();

				int lastId = lastRowId.Id;

				//set new values
				project.ProjectId = lastId;
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
		/// Update project in the database
		/// </summary>
		/// <param name="project"></param>
		public void UpdateProject(Project project)
		{
			if (project == null)
			{
				throw new ArgumentNullException("project must not be null");
			}

			if (db == null)
			{
				return;
			}
			try
			{
				db.BeginTransaction();
				SQLiteCommand cmd = db.CreateCommand(SQL_PROJECT_UPDATE,
													  project.Name,
													  project.Description,
													  project.ProjectId);
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

		public void InsertOrUpdateProject(Project project)
		{
			if (project == null)
			{
				throw new ArgumentNullException("project must not be null");
			}

			int projectId = project.ProjectId;
			if (projectId == 0)
			{
				this.InsertProjectAndSync(project);
			}
			else
			{
				this.UpdateProject(project);
			}
		}

		/// <summary>
		/// Delete project from the database
		/// </summary>
		/// <param name="project"></param>
		public void DeleteProject(Project project)
		{
			if (project == null)
			{
				throw new ArgumentNullException("project must not be null");
			}

			int projectId = project.ProjectId;

			this.DeleteProjectById(projectId);
		}

		/// <summary>
		/// Delete project from database by id
		/// </summary>
		/// <param name="projectId"></param>
		public void DeleteProjectById(int projectId)
		{
			if (db == null)
			{
				return;
			}
			try
			{
				db.BeginTransaction();
				SQLiteCommand cmd = db.CreateCommand(SQL_PROJECT_DELETE_BY_ID,
													  projectId);
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
		/// Returns all projects from the database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Project> GetAllProjects()
		{
			if (db == null)
			{
				return null;
			}

			try
			{
				SQLiteCommand cmd = db.CreateCommand(SQL_PROJECT_SELECT_ALL);
				var projects = cmd.ExecuteQuery<Project>();
				return projects;
			}
			catch (SQLiteException ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Returns single project from the database by id
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns>Project</returns>
		public Project GetProjectById(int projectId)
		{
			if (db == null)
			{
				return null;
			}

			try
			{
				SQLiteCommand cmd = db.CreateCommand(SQL_PROJECT_SELECT_BY_ID,
														projectId);
				var projects = cmd.ExecuteQuery<Project>();
				var project = projects.FirstOrDefault();
				return project;
			}
			catch (SQLiteException ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Creats the Projects table if not exists in the database
		/// </summary>
		public void CreateProjectsTableIfNotExists()
		{
			if (db == null)
			{
				return;
			}
			try
			{
				db.BeginTransaction();
				SQLiteCommand cmd = db.CreateCommand(
							SQL_PROJECT_TABLE_CREATE);
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
		/// Drops the projects table if exists in the database
		/// </summary>
		public void DropProjectsTableIfExists()
		{
			if (db == null)
			{
				return;
			}
			try
			{
				db.BeginTransaction();
				SQLiteCommand cmd = db.CreateCommand(
							SQL_PROJECT_TABLE_DROP);
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


