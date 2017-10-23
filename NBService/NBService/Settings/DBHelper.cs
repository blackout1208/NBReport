using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NBService.Settings
{
    /// <summary>
    /// Helper to Database
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// Sets connection to Reporting Server
        /// </summary>
        public static void ConnectReportingServer()
        {
            //Connection string
            SettingsConnection.ReportingConnString = ConfigurationManager.ConnectionStrings["NBConnectionString"].ConnectionString;
            //Connecting DB
            SettingsConnection.ReportingConn = SettingsConnection.ReportingConn.DbConnect(SettingsConnection.ReportingConnString);
        }

        /// <summary>
        /// Open the connection
        /// </summary>
        /// <param name="sqlConnection">Sql connection</param>
        /// <param name="connectionString">The connection string</param>
        /// <returns>Sql connection initialized</returns>
        public static SqlConnection DbConnect(this SqlConnection sqlConnection, string connectionString)
        {
            if (sqlConnection == null)
            {
                throw new ArgumentNullException(nameof(sqlConnection));
            }
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR :: " + e.Message);
            }

            return null;
        }

        /// <summary>
        /// Closing the connection
        /// </summary>
        /// <param name="sqlConnection">Sql Connection</param>
        public static void DbClose(this SqlConnection sqlConnection)
        {
            try
            {
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR :: " + e.Message);
            }
        }

        /// <summary>
        /// Execution
        /// </summary>
        /// <param name="sqlConnection">Sql Connection</param>
        /// <param name="queryString">Query string to be executed</param>
        /// <returns>The result of the query as a table</returns>
        public static DataTable ExecuteQuery(this SqlConnection sqlConnection, string queryString)
        {
            DataSet dataset;
            try
            {
                //Checking the state of the connection
                if (sqlConnection == null || ((sqlConnection != null &&
                                               (sqlConnection.State == ConnectionState.Closed ||
                                                sqlConnection.State == ConnectionState.Broken))))
                {
                    sqlConnection.Open();
                }

                var dataAdaptor = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryString, sqlConnection) {CommandType = CommandType.Text}
                };

                dataset = new DataSet();
                dataAdaptor.Fill(dataset, "table");
                sqlConnection.Close();
                return dataset.Tables["table"];
            }
            catch (Exception e)
            {
                sqlConnection?.Close();
                Console.WriteLine("ERROR :: " + e.Message);
                return null;
            }
            finally
            {
                sqlConnection?.Close();
            }
        }

        public static DataTable ExecuteProcWithParamsDT(this SqlConnection Conn, string procname, Hashtable parameters)
        {
            DataSet dataSet;
            try
            {
                var dataAdaptor = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(procname, Conn) {CommandType = CommandType.StoredProcedure}
                };
                if (parameters != null)
                {
                    foreach (DictionaryEntry de in parameters)
                    {
                        var sp = new SqlParameter(de.Key.ToString(), de.Value.ToString());
                        dataAdaptor.SelectCommand.Parameters.Add(sp);
                    }
                }

                dataSet = new DataSet();
                dataAdaptor.Fill(dataSet, "table");
                Conn.Close();
                return dataSet.Tables["table"];
            }
            catch (Exception e)
            {
                Conn.Close();
                Console.WriteLine("ERROR :: " + e.Message);
                return null;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}