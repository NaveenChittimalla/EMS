using Microsoft.Data.SqlClient;
using System.Data;

namespace EMS.CoreLibrary.Helpers
{
    /// <summary>
    /// Simple SqlHelper to demonstrate advantage of methods - code organisation and reusability. Not production ready
    /// </summary>
    public sealed class SqlHelper
    {
        // Since this class provides only static methods, make the default constructor private to prevent 
        // instances from being created with "new SqlHelper()"
        private SqlHelper() { }


        /// <summary>
        /// This method is used to attach array of SqlParameters to a SqlCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of SqlParameters to be added to command</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                command.Parameters.Clear();
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private static SqlParameter[] ConvertToSqlParameters(IDictionary<string, object> parameters)
        {
            if (parameters is null || parameters?.Count <= 0) return null;

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach (var parameter in parameters)
            {
                ArgumentNullException.ThrowIfNullOrEmpty(parameter.Key);
                SqlParameter sqlParameter = new SqlParameter($"@{parameter.Key}", parameter.Value);
                sqlParameters.Add(sqlParameter);
            }

            return sqlParameters.ToArray();
        }

        public static int ExecuteNonQuery(string connectionString, string storedProcedureName, IDictionary<string, object>? parameters = null)
        {
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, storedProcedureName, parameters);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, IDictionary<string, object>? parameters = null)
        {
            ArgumentNullException.ThrowIfNull(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    var sqlParameters = ConvertToSqlParameters(parameters);
                    if (sqlParameters != null)
                    {
                        AttachParameters(command, sqlParameters);
                    }

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string connectionString, string storedProcedureName, IDictionary<string, object>? parameters = null)
        {
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedureName, parameters);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, IDictionary<string, object>? parameters = null)
        {
            ArgumentNullException.ThrowIfNull(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    var sqlParameters = ConvertToSqlParameters(parameters);
                    if (sqlParameters != null)
                    {
                        AttachParameters(command, sqlParameters);
                    }

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        public static DataSet ExecuteDataSet(string connectionString, string storedProcedureName, IDictionary<string, object>? parameters = null)
        {
            return ExecuteDataSet(connectionString, CommandType.StoredProcedure, storedProcedureName, parameters);
        }

        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText, IDictionary<string, object>? parameters = null)
        {
            ArgumentNullException.ThrowIfNull(connectionString);
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    var sqlParameters = ConvertToSqlParameters(parameters);
                    if (sqlParameters != null)
                    {
                        AttachParameters(command, sqlParameters);
                    }

                    using (SqlDataAdapter adaptor = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        adaptor.Fill(ds);
                    }
                    return ds;
                }
            }
        }
    }
}
