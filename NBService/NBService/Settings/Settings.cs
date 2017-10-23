using System.Data.SqlClient;

namespace NBService.Settings
{
    /// <summary>
    /// Database Settings
    /// </summary>
    public class SettingsConnection
    {

        /// <summary>
        /// Connection String to Report Database
        /// </summary>
        public static string ReportingConnString { get; set; }

        /// <summary>
        /// Sql Connection to Database
        /// </summary>
        public static SqlConnection ReportingConn { get; set; }

    }
}