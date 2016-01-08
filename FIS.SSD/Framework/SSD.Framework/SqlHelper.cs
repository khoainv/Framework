using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using SSD.Framework.Collections;

namespace SSD.Framework
{
    public abstract class BaseEntity<T> where T : new()
    {
        static object obj = new object();
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        instance = new T();
                    }
                }
                return instance;
            }
        }
        public abstract SortableBindingList<T> TransferDataToObjects(IDataReader reader, bool reading = false, bool usingCacheMapColIndex = true);
        public abstract T TransferDataToObject(DataRow dr);
        public abstract SortableBindingList<T> TransferDataToObjects(DataTable dataTable);
        public HashSet<string> ListColumnName(IDataReader reader)
        {
            HashSet<string> lstCol = new HashSet<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                lstCol.Add(reader.GetName(i));
            }
            return lstCol;
        }
    }
    public class SqlHelper
    {
        public static bool IsSQL2012
        {
            get
            {
                return ConfigurationManager.AppSettings["VersionSQL"] == "2012";
            }
        }

        #region utilities

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Attach an array of sql parameters to a command
        /// </summary>
        public static void AttachParameters(SqlCommand command, SqlParameter[] sqlParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (sqlParameters != null)
            {
                foreach (SqlParameter sqlParameter in sqlParameters)
                {
                    if (sqlParameter != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((sqlParameter.Direction == ParameterDirection.InputOutput ||
                            sqlParameter.Direction == ParameterDirection.Input) &&
                            (sqlParameter.Value == null))
                        {
                            sqlParameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(sqlParameter);
                    }
                }
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Prepare a command by attaching parameters to it
        /// </summary>
        public static SqlCommand PrepareCommand(string commandText, SqlParameter[] sqlParameters)
        {
            SqlCommand command = null;

            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            command = new SqlCommand(commandText);
            AttachParameters(command, sqlParameters);

            return command;
        }


        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Create a sql parameter
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <returns>SqlParameter.</returns>
        public static SqlParameter CreateSqlPrameter(string parameterName, object value)
        {
            SqlParameter param = new SqlParameter(parameterName, value);
            return param;
        }


        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Create a sql parameter
        /// </summary>
        public static SqlParameter CreateSqlPrameter(string parameterName, SqlDbType dbType, int size, object value)
        {
            SqlParameter param = new SqlParameter(parameterName, dbType, size);
            param.Value = value;
            return param;
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Create a sql parameter
        /// </summary>
        public static SqlParameter CreateSqlPrameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(parameterName, dbType, size);
            param.Direction = direction;
            return param;
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Create a sql parameter
        /// </summary>
        public static SqlParameter CreateSqlPrameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction,object value)
        {
            SqlParameter param = new SqlParameter(parameterName, dbType, size);
            param.Direction = direction;
            param.Value = value;
            return param;
        }
        #endregion

        #region excute queries

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute scalar query
        /// </summary>
        public static object ExecuteScalar(SqlConnection connection, string commandText, SqlParameter[] sqlParameters)
        {
            SqlCommand command = PrepareCommand(commandText, sqlParameters);

            try
            {
                command.Connection = connection;

                return command.ExecuteScalar();
            }
            finally
            {
                command.Parameters.Clear();
                //if (connection != null && connection.State != ConnectionState.Closed)
                //{
                //    connection.Close();
                //}
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a scalar query
        /// </summary>
        public static object ExecuteScalar(SqlConnection connection, string commandText)
        {
            return ExecuteScalar(connection, commandText, null);
        }
        public static IDataReader ExecuteReader(SqlConnection connection, string commandText, SqlParameter[] sqlParameters = null, bool isStoreProcedure = false)
        {
            SqlCommand command = PrepareCommand(commandText, sqlParameters);
            try
            {
                command.Connection = connection;
                if (isStoreProcedure)
                    command.CommandType = CommandType.StoredProcedure;

                return command.ExecuteReader();
            }
            finally
            {
                command.Parameters.Clear();
            }
        }
        public static async Task<IDataReader> ExecuteReaderAsync(SqlConnection connection, string commandText, SqlParameter[] sqlParameters = null, bool isStoreProcedure = false)
        {
            SqlCommand command = PrepareCommand(commandText, sqlParameters);
            try
            {
                command.Connection = connection;
                if (isStoreProcedure)
                    command.CommandType = CommandType.StoredProcedure;

                return await command.ExecuteReaderAsync();
            }
            finally
            {
                command.Parameters.Clear();
            }
        }
        public static SortableBindingList<T> ExecuteReader<T>(SqlConnection connection, string commandText, SqlParameter[] sqlParameters = null, bool isStoreProcedure = false, bool usingCacheMapColIndex = true) where T : BaseEntity<T>, new()
        {
            SortableBindingList<T> lst = new SortableBindingList<T>();
            using (IDataReader reader = SqlHelper.ExecuteReader((SqlConnection)connection, commandText, sqlParameters, isStoreProcedure))
            {
                lst = BaseEntity<T>.Instance.TransferDataToObjects(reader, usingCacheMapColIndex: usingCacheMapColIndex);
            }
            return lst;
        }
        public static async Task<SortableBindingList<T>> ExecuteReaderAsync<T>(SqlConnection connection, string commandText, SqlParameter[] sqlParameters = null, bool isStoreProcedure = false, bool usingCacheMapColIndex = true) where T : BaseEntity<T>, new()
        {
            SortableBindingList<T> lst = new SortableBindingList<T>();
            using (IDataReader reader = await SqlHelper.ExecuteReaderAsync((SqlConnection)connection, commandText, sqlParameters, isStoreProcedure))
            {
                lst = BaseEntity<T>.Instance.TransferDataToObjects(reader, usingCacheMapColIndex: usingCacheMapColIndex);
            }
            return lst;
        }
        public static T ExecuteReaderOne<T>(SqlConnection connection, string commandText, SqlParameter[] sqlParameters = null, bool isStoreProcedure = false, bool usingCacheMapColIndex = true) where T : BaseEntity<T>, new()
        {
            var lst = ExecuteReader<T>(connection, commandText, sqlParameters, isStoreProcedure, usingCacheMapColIndex);
            if (lst.Count > 0)
                return lst.First();
            return null;
        }
        public static async Task<T> ExecuteReaderOneAsync<T>(SqlConnection connection, string commandText, SqlParameter[] sqlParameters = null, bool isStoreProcedure = false, bool usingCacheMapColIndex = true) where T : BaseEntity<T>, new()
        {
            var lst = await ExecuteReaderAsync<T>(connection, commandText, sqlParameters, isStoreProcedure, usingCacheMapColIndex);
            if (lst.Count > 0)
                return lst.First();
            return null;
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a query
        /// </summary>
        public static DataSet ExecuteQuery(SqlConnection connection, string commandText, SqlParameter[] sqlParameters, bool isStoreProcedure)
        {
            SqlCommand command = PrepareCommand(commandText, sqlParameters);

            try
            {
                command.Connection = connection;
                if (isStoreProcedure)
                    command.CommandType = CommandType.StoredProcedure;
                DataSet dataSet = new DataSet();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    dataAdapter.Fill(dataSet);
                }
                return dataSet;
            }
            finally
            {
                command.Parameters.Clear();
                //if (connection != null && connection.State != ConnectionState.Closed)
                //{
                //    connection.Close();
                //}
            }
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a query
        /// </summary>
        public static DataSet ExecuteQuery(SqlConnection connection, string commandText, SqlParameter[] sqlParameters)
        {
            return ExecuteQuery(connection, commandText, sqlParameters,false);
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a query
        /// </summary>
        public static DataSet ExecuteQuery(SqlConnection connection, string commandText)
        {
            return ExecuteQuery(connection, commandText, null);
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a query
        /// </summary>
        public static DataSet ExecuteQuery(SqlCommand command)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    dataAdapter.Fill(dataSet);
                }
                return dataSet;
            }
            finally
            {
                command.Parameters.Clear();
                //if (command != null && command.Connection.State != ConnectionState.Closed)
                //{
                //    command.Connection.Close();
                //}
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a none query to the db (Insert, update, delete)
        /// </summary>
        public static int ExecuteNonQuery(SqlTransaction transaction, string commandText, SqlParameter[] sqlParameters)
        {
            return ExecuteNonQuery(transaction, commandText, sqlParameters, false);
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a none query to the db (Insert, update, delete)
        /// </summary>
        public static int ExecuteNonQuery(SqlTransaction transaction, string commandText, SqlParameter[] sqlParameters, bool isStoreProcedure)
        {
            SqlCommand command = PrepareCommand(commandText, sqlParameters);
            try
            {
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
                if (isStoreProcedure)
                    command.CommandType = CommandType.StoredProcedure;
                return command.ExecuteNonQuery();
            }
            finally
            {
                command.Parameters.Clear();
            }
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a none query to the db (Insert, update, delete)
        /// </summary>
        public static int ExecuteNonQuery(SqlConnection connection, string commandText, SqlParameter[] sqlParameters)
        {
            return ExecuteNonQuery(connection, commandText, sqlParameters, false);
        }
        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a none query to the db (Insert, update, delete)
        /// </summary>
        public static int ExecuteNonQuery(SqlConnection connection, string commandText, SqlParameter[] sqlParameters,bool isStoreProcedure)
        {
            SqlCommand command = PrepareCommand(commandText, sqlParameters);
            try
            {
                command.Connection = connection;
                if (isStoreProcedure)
                    command.CommandType = CommandType.StoredProcedure;
                return command.ExecuteNonQuery();
            }
            finally
            {
                command.Parameters.Clear();
                //if (connection != null && connection.State != ConnectionState.Closed)
                //{
                //    connection.Close();
                //}
            }
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a none query to the db (Insert, update, delete)
        /// </summary>
        public static int ExecuteNonQuery(SqlTransaction transaction, string commandText)
        {
            return ExecuteNonQuery(transaction, commandText, null);
        }

        /// <summary>
        /// Author: Nguyen Duc Thuan
        /// Todo: Execute a none query to the db (Insert, update, delete)
        /// </summary>
        public static int ExecuteNonQuery(SqlConnection connection, string commandText)
        {
            return ExecuteNonQuery(connection, commandText, null);
        }

        #endregion
    }
}
