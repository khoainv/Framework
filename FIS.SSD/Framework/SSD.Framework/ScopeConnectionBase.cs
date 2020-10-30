using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Configuration;
using System.Data;
using SSD.Framework.Exceptions;

namespace SSD.Framework
{
    public class ScopeConnectionBase
    {
        public ScopeConnectionBase(string connectionStringName)
        {
            ConnectionStringName = connectionStringName;
        }
        public ScopeConnectionBase(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
        }
        public string ConnectionStringName { get; set; }
        private string connStr;
        public string ConnectionString
        {
            get {
                if(connStr==null)
                    connStr = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

                return connStr;
            }
            set
            {
                connStr = value;
            }
        }
        private string provider;
        public string ProviderName
        {
            get
            {
                if (provider == null)
                    provider = ConfigurationManager.ConnectionStrings[ConnectionStringName].ProviderName;

                return provider;
            }
            set
            {
                provider = value;
            }
        }
        IDbConnection _conn;
        public void CloseConnection()
        {
            if (_conn.State != ConnectionState.Closed)
                _conn.Close();
        }
        public void GetConnectionAndOpen(ref IDbConnection conn, string connStr)
        {
            if (conn == null)
            {
                conn = GetConnection(conn, connStr);
            }
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw new OpenConnectionSQLException(ex);
                }
            }
        }
        public IDbConnection GetConnection(IDbConnection conn, string connStr)
        {
            if (conn == null)
            {
                conn = DbProviderFactories.GetFactory(ProviderName).CreateConnection();
                conn.ConnectionString = connStr;
            }
            return conn;
        }
        //With Transaction
        public IDbConnection ConnectionWithTransaction
        {
            get
            {
                //Open pool connection with Transaction
                IDbConnection conn = null;
                GetConnectionAndOpen(ref conn, ConnectionString);
                return conn;
            }
        }
        public IDbConnection Connection(out bool isNew)
        {
            //Open pool connection
            IDbConnection conn = null;
            isNew = false;
            if (_conn == null)
            {
                GetConnectionAndOpen(ref _conn, ConnectionString);
                return _conn;
            }
            else
            if (_conn.State == ConnectionState.Open || _conn.State == ConnectionState.Closed)
            {
                conn = _conn;
            }
            else
                isNew = true;
            GetConnectionAndOpen(ref conn, ConnectionString);
            return conn;
        }
    }
}
