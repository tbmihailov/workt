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

namespace WorkT.Data
{
    public class DatabaseProvider
    {
        private static string _databaseName;
        public static string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }
        
        public static SQLiteClient.SQLiteConnection GetSqlConnection()
        {
            string databaseName = DatabaseName;
            var sqlConnection = new SQLiteClient.SQLiteConnection(databaseName);
            
            return sqlConnection;
        }
    }
}
