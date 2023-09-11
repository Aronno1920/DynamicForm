using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;

namespace CoreLibrary
{
    public class DataManager
    {
        private SqlConnection sqlConnection;

        private SqlTransaction sqlTransaction;

        private string connectionString;

        public DataManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConString"].ToString();
            sqlConnection = new SqlConnection(connectionString);
        }

        #region Database Connection Related
        public void OpenConnection()
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (sqlConnection != null)
            {
                sqlConnection.Close();
            }
        }
        #endregion

        #region Data Transaction Related
        public void BeginTransaction()
        {
            OpenConnection();
            if (sqlTransaction == null)
            {
                sqlTransaction = sqlConnection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (sqlTransaction != null)
            {
                sqlTransaction.Commit();
            }
        }

        public void RollbackTransaction()
        {
            if (sqlTransaction != null)
            {
                sqlTransaction.Rollback();
            }
        }
        #endregion

        #region Get Data from Database
        public DataTable GetDataTable(string query)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            sqlDataAdapter.Dispose();
            return dataTable;
        }

        public DataTable GetDataTable(string query, SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = CreateCommand(query, parameters);
            sqlCommand.Connection = sqlConnection;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            sqlDataAdapter.Dispose();
            return dataTable;
        }

        public DataSet GetDataSet(string query)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            sqlDataAdapter.Dispose();
            return dataSet;
        }

        public DataSet GetDataSet(string query, SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = CreateCommand(query, parameters);
            sqlCommand.Connection = sqlConnection;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            sqlDataAdapter.Dispose();
            return dataSet;
        }
        #endregion

        #region Create SQL Command
        private SqlCommand CreateCommandQuery(string query, SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = ((sqlTransaction != null) ? new SqlCommand(query, sqlConnection, sqlTransaction) : new SqlCommand(query, sqlConnection));
            sqlCommand.CommandTimeout = 100000;
            sqlCommand.CommandType = CommandType.Text;
            if (parameters != null)
            {
                foreach (SqlParameter value in parameters)
                {
                    sqlCommand.Parameters.Add(value);
                }
            }

            sqlCommand.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, isNullable: false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return sqlCommand;
        }

        private SqlCommand CreateCommand(string procName, SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = ((sqlTransaction != null) ? new SqlCommand(procName, sqlConnection, sqlTransaction) : new SqlCommand(procName, sqlConnection));
            sqlCommand.CommandTimeout = 100000;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (SqlParameter value in parameters)
                {
                    sqlCommand.Parameters.Add(value);
                }
            }

            sqlCommand.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, isNullable: false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return sqlCommand;
        }
        #endregion

        #region Others
        public int RunProc(string query)
        {
            SqlCommand sqlCommand = CreateCommand(query, null);
            sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return (int)sqlCommand.Parameters["ReturnValue"].Value;
        }

        public string RunProcedure(string query)
        {
            OpenConnection();
            SqlCommand sqlCommand = CreateCommand(query, null);
            sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return (string)sqlCommand.Parameters["@returnParam"].Value;
        }

        public int RunQuery(string query)
        {
            return new SqlCommand(query, sqlConnection, sqlTransaction).ExecuteNonQuery();
        }

        public int ExecuteProc(string query, SqlParameter[] parameters)
        {
            return CreateCommand(query, parameters).ExecuteNonQuery();
        }

        public string ExecuteProcWithMsg(string query, SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = CreateCommand(query, parameters);
            sqlCommand.ExecuteNonQuery();
            return (string)sqlCommand.Parameters["@ReturnParam"].Value;
        }

        public bool IsExist(string query)
        {
            return new SqlCommand(query, sqlConnection, sqlTransaction).ExecuteScalar() != null;
        }

        public object ExecuteScalar(string query)
        {
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            OpenConnection();
            return sqlCommand.ExecuteScalar();
        }

        public object ExecuteScalar2(string query)
        {
            return new SqlCommand(query, sqlConnection, sqlTransaction).ExecuteScalar();
        }

        public int RunProc(string query, SqlParameter[] prams)
        {
            int result = 0;
            try
            {
                BeginTransaction();
                SqlCommand sqlCommand = CreateCommand(query, prams);
                sqlCommand.ExecuteNonQuery();
                CommitTransaction();
                result = (int)sqlCommand.Parameters["ReturnValue"].Value;
            }
            catch (SqlException)
            {
                RollbackTransaction();
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

        public string RunProcedure(string query, SqlParameter[] prams)
        {
            string result = string.Empty;
            try
            {
                BeginTransaction();
                SqlCommand sqlCommand = CreateCommand(query, prams);
                sqlCommand.ExecuteNonQuery();
                CommitTransaction();
                result = (string)sqlCommand.Parameters["@return_param"].Value;
            }
            catch (SqlException)
            {
                RollbackTransaction();
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

        public void RunProc(string query, out SqlDataReader dataReader)
        {
            OpenConnection();
            SqlCommand sqlCommand = CreateCommand(query, null);
            dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        #region SQL Data Reader Related
        public SqlDataReader ExecuteReaderWithQuery(string query, SqlParameter[] prams)
        {
            OpenConnection();
            return CreateCommandQuery(query, prams).ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader ExecuteReader(string query, SqlParameter[] prams)
        {
            OpenConnection();
            return CreateCommand(query, prams).ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        #region SQL Param Related
        public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, byte SizeBeforeDecimal, byte SizeAfterDecimal, object Value)
        {
            return MakeParam(ParamName, DbType, SizeBeforeDecimal, SizeAfterDecimal, ParameterDirection.Input, Value);
        }

        public SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public SqlParameter MakeParam(string ParamName, SqlDbType DbType, int Size, ParameterDirection direction, object value)
        {
            SqlParameter sqlParameter = ((Size <= 0) ? new SqlParameter(ParamName, DbType) : new SqlParameter(ParamName, DbType, Size));
            sqlParameter.Direction = direction;
            if (direction != ParameterDirection.Output || value != null)
            {
                sqlParameter.Value = value;
            }

            return sqlParameter;
        }

        public SqlParameter MakeParam(string ParamName, SqlDbType DbType, byte Precision, byte Scale, ParameterDirection direction, object value)
        {
            SqlParameter sqlParameter = ((Precision <= 0) ? new SqlParameter(ParamName, DbType) : new SqlParameter(ParamName, DbType));
            sqlParameter.Precision = Precision;
            sqlParameter.Scale = Scale;
            sqlParameter.Direction = direction;

            if (direction != ParameterDirection.Output || value != null)
            {
                sqlParameter.Value = value;
            }

            return sqlParameter;
        }
        #endregion

        public int ExecuteNonQuery(String strSQL)
        {
            OpenConnection();
            try
            {

                SqlCommand Cmnd = new SqlCommand(strSQL, sqlConnection);

                int affectedRow = Cmnd.ExecuteNonQuery();

                return affectedRow;
            }
            catch (SqlException Ex)
            {

                throw Ex;
            }
            catch(Exception ex)
            {

                throw ex;
            }
        }

        public void ExecuteProcedure(string query, SqlParameter[] parameters)
        {
            OpenConnection();
            try
            {
                SqlCommand sqlCommand = CreateCommand(query, parameters);
            sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException Ex)
            {

                throw Ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable GetDataTableByStoredProcedure(String spName, Hashtable parameters)
        {
            OpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(spName, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (string parametername in parameters.Keys)
                    {
                        cmd.Parameters.AddWithValue(parametername, parameters[parametername]);
                    }
                }

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                adp.Fill(ds);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch(Exception ex)
            {
                CloseConnection();
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}