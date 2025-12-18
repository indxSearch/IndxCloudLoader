using DotNetEnv;

namespace IndxCloudLoader
{
    internal partial class Program
    {
        #region Private Fields
        private static readonly string uri;
        private static readonly string bearerToken;
        private static readonly string userEmail;
        private static readonly string userPassword;
        #endregion Private Fields

        #region Private Methods
        static Program()
        {
            // Load environment variables from .env.local file
            Env.Load(".env.local");

            uri = Environment.GetEnvironmentVariable("API_URI") ?? "https://localhost:5001/";
            bearerToken = Environment.GetEnvironmentVariable("BEARER_TOKEN") ?? "";
            userEmail = Environment.GetEnvironmentVariable("USER_EMAIL") ?? "";
            userPassword = Environment.GetEnvironmentVariable("USER_PASSWORD") ?? "";
        }

        private static void Main()
        {
            LoadDataset().Wait(-1);
        }
        #endregion Private Methods
    }
}