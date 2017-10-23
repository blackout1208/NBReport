using System.ServiceModel;

namespace NBService
{
    /// <summary>
    /// Interface for the Service
    /// </summary>
    [ServiceContract]
    public interface IServices
    {

        /// <summary>
        /// To Create the Test cycle
        /// </summary>
        /// <param name="dbConnectionString">Database connection string</param>
        /// <param name="executedBy">Who executed the test</param>
        /// <param name="requestedBy">Who requested the service</param>
        /// <param name="buildNo">Build number</param>
        /// <param name="appVersion">Application version</param>
        /// <param name="machineName">Operative System</param>
        [OperationContract]
        void CreateTestCycle(string dbConnectionString, string executedBy, string requestedBy, string buildNo, string appVersion, string machineName);

        /// <summary>
        /// To Write the result from the test
        /// </summary>
        /// <param name="featureName"></param>
        /// <param name="scenarioName"></param>
        /// <param name="stepName"></param>
        /// <param name="exception"></param>
        /// <param name="result"></param>
        [OperationContract]
        void WriteTestResult(string featureName, string scenarioName, string stepName, string exception, string result);
    }
}
