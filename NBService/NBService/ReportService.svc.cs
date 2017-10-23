using System;
using System.Collections;
using System.Configuration;
using NBService.Settings;

namespace NBService
{
    /// <summary>
    /// Main Service class which is consumed by the EAFramework or any custom framework
    /// </summary>
    public class ReportService : IServices
    {
        public ReportService()
        {
            SettingsConnection.ReportingConnString = ConfigurationManager.ConnectionStrings["NBConnectionString"].ConnectionString;
            SettingsConnection.ReportingConn = SettingsConnection.ReportingConn.DbConnect(SettingsConnection.ReportingConnString);
        }

        /// <summary>
        /// Creates a test cycle
        /// </summary>
        /// <param name="connection">Connection string</param>
        /// <param name="executedBy">Who executed the test</param>
        /// <param name="requestedBy">Who requested the test</param>
        /// <param name="buildNo">Build number</param>
        /// <param name="appVersion">Version of test</param>
        /// <param name="machineName">Operative System</param>
        public void CreateTestCycle(string connection, string executedBy, string requestedBy, string buildNo, string appVersion, string machineName)
        {
            try
            {
                Hashtable table = new Hashtable
                {
                    {"AUT", connection},
                    {"ExecutedBy", executedBy},
                    {"RequestedBy", requestedBy},
                    {"BuildNo", buildNo},
                    {"ApplicationVersion", appVersion},
                    {"MachineName", machineName},
                    {"TestType", 1}
                };
                //TODO: Extend to TestType
                // SmokeTest = 1
                // RegressionTest = 2
                // IntegrationTest = 3
                SettingsConnection.ReportingConn.ExecuteProcWithParamsDT("sp_CreateTestCycleID", table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Writes the test Result
        /// </summary>
        /// <param name="featureName">Feature Name</param>
        /// <param name="scenarioName">Scenario Name</param>
        /// <param name="stepName">Step Name</param>
        /// <param name="exception">Stack trace exception</param>
        /// <param name="result">Result</param>
        public void WriteTestResult(string featureName, string scenarioName, string stepName, string exception, string result)
        {
            try
            {
                var table = new Hashtable
                {
                    {"FeatureName", featureName},
                    {"ScenarioName", scenarioName},
                    {"StepName", stepName},
                    {"Exception", exception},
                    {"Result", result}
                };
                SettingsConnection.ReportingConn.ExecuteProcWithParamsDT("sp_InsertResult", table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
