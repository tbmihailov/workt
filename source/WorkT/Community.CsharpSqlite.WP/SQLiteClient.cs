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
using System.Collections.Generic;
using Community.CsharpSqlite;


namespace SQLiteClient
{
    public class SQLiteException : Exception
    {
        public SQLiteException(string message)
            : base(message)
        {
        }
    }

    public class SQLiteConnection : IDisposable
    {
        private Sqlite3.sqlite3 _db;
        private bool _open;
        public string callbackError;

        public string Database { get; set; }

        public bool TransactionOpened = false;

        public SQLiteConnection(string database)
        {
            Database = database;
        }

        int callback(object pArg, System.Int64 nArg, object azArgs, object azCols)
        {
            int i;
            string[] azArg = (string[])azArgs;
            string[] azCol = (string[])azCols;
            String sb = "";// = new String();
            for (i = 0; i < nArg; i++)
                sb += azCol[i] + " = " + azArg[i] + "\n";
            callbackError += sb.ToString();
            return 0;
        }

        public void Open()
        {
            if (Sqlite3.sqlite3_open(Database, ref _db) != Sqlite3.SQLITE_OK)
                throw new SQLiteException("Could not open database file: " + Database);
            string errMsg = string.Empty;
            if (Sqlite3.sqlite3_exec(_db, "PRAGMA journal_mode=PERSIST", (Sqlite3.dxCallback)this.callback, null, ref errMsg) != Sqlite3.SQLITE_OK)
            {
                Sqlite3.sqlite3_close(_db);
                _db = null;
                _open = false;
                throw new SQLiteException("Cannot set journal mode to PERSIST: " + Database);
            }
            _open = true;
        }

        public SQLiteCommand CreateCommand(string cmdText, params object[] ps)
        {
            if (!_open)
            {
                throw new SQLiteException("Cannot create commands from unopened database");
            }
            else
            {
                var cmd = new SQLiteCommand(_db);
                cmd.CommandText = cmdText;
                foreach (var o in ps)
                {
                    cmd.Bind(o);
                }
                return cmd;
            }
        }

        public SQLiteCommand CreateCommand(string cmdText)
        {
            if (!_open)
            {
                throw new SQLiteException("Cannot create commands from unopened database");
            }
            else
            {
                var cmd = new SQLiteCommand(_db);
                cmd.CommandText = cmdText;
                return cmd;
            }
        }

        public int Execute(string query, params object[] ps)
        {
            var cmd = CreateCommand(query, ps);
            Console.Error.WriteLine("Executing " + cmd);
            return cmd.ExecuteNonQuery();
        }

        public bool BeginTransaction()
        {
            if (Sqlite3.sqlite3_exec(_db, "BEGIN", 0, 0, 0) != Sqlite3.SQLITE_OK)
            {
                string msgC = Sqlite3.sqlite3_errmsg(_db);
                throw new SQLiteException(msgC);
            }
            TransactionOpened = true;
            return true;
        }

        public bool RollbackTransaction()
        {
            if (Sqlite3.sqlite3_exec(_db, "ROLLBACK", 0, 0, 0) != Sqlite3.SQLITE_OK)
            {
                string msgC = Sqlite3.sqlite3_errmsg(_db);
                throw new SQLiteException(msgC);
            }
            TransactionOpened = false;
            return true;
        }

        public bool CommitTransaction()
        {
            if (Sqlite3.sqlite3_exec(_db, "COMMIT", 0, 0, 0) != Sqlite3.SQLITE_OK)
            {
                string msgC = Sqlite3.sqlite3_errmsg(_db);
                throw new SQLiteException(msgC);
            }
            TransactionOpened = false;
            return true;
        }


        public IEnumerable<T> Query<T>(string query, params object[] ps) where T : new()
        {
            var cmd = CreateCommand(query, ps);
            return cmd.ExecuteQuery<T>();
        }

        public void Dispose()
        {
            if (_open)
            {
                Sqlite3.sqlite3_close(_db);
                _db = null;
                _open = false;
            }
        }
    }

    public class SQLiteCommand
    {
        private Sqlite3.sqlite3 _db;
        private List<Binding> _bindings;

        public string CommandText { get; set; }

        internal SQLiteCommand(Sqlite3.sqlite3 db)
        {
            _db = db;
            _bindings = new List<Binding>();
            CommandText = "";
        }

        public int ExecuteNonQueryEx()
        {
            if (Sqlite3.sqlite3_exec(_db, CommandText, 0, 0, 0) != Sqlite3.SQLITE_OK)
            {
                string msgC = Sqlite3.sqlite3_errmsg(_db);
                throw new SQLiteException(msgC);
            }
            return 1;
        }

        public int ExecuteNonQuery()
        {
            Sqlite3.Vdbe stmt = Prepare();

            var r = Sqlite3.sqlite3_step(stmt);
            switch (r)
            {
                case Sqlite3.SQLITE_ERROR:
                    string msg = Sqlite3.sqlite3_errmsg(_db);
                    Sqlite3.sqlite3_finalize(ref stmt);
                    throw new SQLiteException(msg);
                case Sqlite3.SQLITE_DONE:
                    int rowsAffected = Sqlite3.sqlite3_changes(_db);
                    Sqlite3.sqlite3_finalize(ref stmt);
                    return rowsAffected;
                case Sqlite3.SQLITE_CANTOPEN:
                    Sqlite3.sqlite3_finalize(ref stmt);
                    throw new SQLiteException("Cannot open database file");
                case Sqlite3.SQLITE_CONSTRAINT:
                    string msgC = Sqlite3.sqlite3_errmsg(_db);
                    Sqlite3.sqlite3_finalize(ref stmt);
                    throw new SQLiteException(msgC);
                default:
                    Sqlite3.sqlite3_finalize(ref stmt);
                    throw new SQLiteException("Unknown error");
            }
        }

        public IEnumerable<T> ExecuteQuery<T>() where T : new()
        {
            var stmt = Prepare();

            var props = GetProps(typeof(T));
            var cols = new System.Reflection.PropertyInfo[Sqlite3.sqlite3_column_count(stmt)];
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i] = MatchColProp(Sqlite3.sqlite3_column_name(stmt, i), props);
            }

            while (Sqlite3.sqlite3_step(stmt) == Sqlite3.SQLITE_ROW)
            {
                var obj = new T();
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i] == null)
                        continue;
                    var val = ReadCol(stmt, i, cols[i].PropertyType);
                    cols[i].SetValue(obj, val, null);
                }
                yield return obj;
            }
            Sqlite3.sqlite3_finalize(ref stmt);
        }

        public void Bind(string name, object val)
        {
            _bindings.Add(new Binding
            {
                Name = name,
                Value = val
            });
        }
        public void Bind(object val)
        {
            Bind(null, val);
        }

        public override string ToString()
        {
            return CommandText;
        }

        Sqlite3.Vdbe Prepare()
        {
            Sqlite3.Vdbe ppStmt = new Sqlite3.Vdbe();
            if (Sqlite3.sqlite3_prepare_v2(_db, CommandText, CommandText.Length, ref ppStmt, 0) != Sqlite3.SQLITE_OK)
                throw new SQLiteException(Sqlite3.sqlite3_errmsg(_db));
            BindAll(ppStmt);
            return ppStmt;
        }

        void BindAll(Sqlite3.Vdbe stmt)
        {
            int nextIdx = 1;
            foreach (var b in _bindings)
            {
                if (b.Name != null)
                {
                    b.Index = Sqlite3.sqlite3_bind_parameter_index(stmt, b.Name);
                }
                else
                {
                    b.Index = nextIdx++;
                }
            }
            foreach (var b in _bindings)
            {
                if (b.Value == null)
                {
                    Sqlite3.sqlite3_bind_null(stmt, b.Index);
                }
                else
                {
                    if (b.Value is Byte || b.Value is UInt16 || b.Value is SByte || b.Value is Int16 || b.Value is Int32)
                    {
                        Sqlite3.sqlite3_bind_int(stmt, b.Index, Convert.ToInt32(b.Value));
                    }
                    else if (b.Value is UInt32 || b.Value is Int64)
                    {
                        Sqlite3.sqlite3_bind_int64(stmt, b.Index, Convert.ToInt64(b.Value));
                    }
                    else if (b.Value is Single || b.Value is Double || b.Value is Decimal)
                    {
                        Sqlite3.sqlite3_bind_double(stmt, b.Index, Convert.ToDouble(b.Value));
                    }
                    else if (b.Value is String)
                    {
                        Sqlite3.sqlite3_bind_text(stmt, b.Index, b.Value.ToString(), -1, null);
                    }
                }
            }
        }

        class Binding
        {
            public string Name { get; set; }
            public object Value { get; set; }
            public int Index { get; set; }
        }

        object ReadCol(Sqlite3.Vdbe stmt, int index, Type clrType)
        {
            var type = Sqlite3.sqlite3_column_type(stmt, index);
            if (type == Sqlite3.SQLITE_NULL)
            {
                return null;
            }
            else
            {
                if (clrType == typeof(Byte) || clrType == typeof(UInt16) || clrType == typeof(SByte) || clrType == typeof(Int16) || clrType == typeof(Int32))
                {
                    return Convert.ChangeType(Sqlite3.sqlite3_column_int(stmt, index), clrType, System.Threading.Thread.CurrentThread.CurrentCulture);
                }
                else if (clrType == typeof(UInt32) || clrType == typeof(Int64))
                {
                    return Convert.ChangeType(Sqlite3.sqlite3_column_int64(stmt, index), clrType, System.Threading.Thread.CurrentThread.CurrentCulture);
                }
                else if (clrType == typeof(Single) || clrType == typeof(Double) || clrType == typeof(Decimal))
                {
                    return Convert.ChangeType(Sqlite3.sqlite3_column_double(stmt, index), clrType, System.Threading.Thread.CurrentThread.CurrentCulture);
                }
                else if (clrType == typeof(String))
                {
                    return Convert.ChangeType(Sqlite3.sqlite3_column_text(stmt, index), clrType, System.Threading.Thread.CurrentThread.CurrentCulture);
                }
                else
                {
                    throw new NotSupportedException("Don't know how to read " + clrType);
                }
            }
        }

        static System.Reflection.PropertyInfo[] GetProps(Type t)
        {
            return t.GetProperties();
        }

        static System.Reflection.PropertyInfo MatchColProp(string colName, System.Reflection.PropertyInfo[] props)
        {
            foreach (var p in props)
            {
                if (p.Name == colName)
                {
                    return p;
                }
            }
            return null;
        }
    }




}
