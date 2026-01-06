using DotNetEnv;
using System.CommandLine;

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

        private static async Task<int> Main(string[] args)
        {
            // Create root command
            var rootCommand = new RootCommand("IndxCloudLoader - Load and configure datasets for IndxCloudApi");

            // Add dataset option
            var datasetOption = new Option<string>(
                aliases: new[] { "--dataset", "-d" },
                description: "Dataset to load (tmdb or pokedex). If not provided, interactive mode will prompt for selection."
            );
            rootCommand.AddOption(datasetOption);

            // Add handler
            rootCommand.SetHandler(async (string dataset) =>
            {
                try
                {
                    // If no dataset provided, show interactive menu
                    if (string.IsNullOrEmpty(dataset))
                    {
                        dataset = ShowInteractiveMenu();
                        if (dataset == null)
                        {
                            ConsoleHelper.WriteWarning("No dataset selected. Exiting.");
                            return;
                        }
                    }

                    await LoadDataset(dataset);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteError($"Fatal error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        ConsoleHelper.WriteError($"  Inner exception: {ex.InnerException.Message}");
                    }
                    Environment.Exit(-1);
                }
            }, datasetOption);

            return await rootCommand.InvokeAsync(args);
        }

        private static string ShowInteractiveMenu()
        {
            ConsoleHelper.WriteHeader("Dataset Selection");
            Console.WriteLine();

            var datasets = DatasetConfig.GetAvailableDatasets();

            Console.WriteLine("Available datasets:");
            for (int i = 0; i < datasets.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {datasets[i]}");
            }
            Console.WriteLine($"  0. Exit");
            Console.WriteLine();
            Console.Write("Select dataset (enter number): ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0)
                    return null;

                if (choice > 0 && choice <= datasets.Length)
                {
                    return datasets[choice - 1];
                }
            }

            ConsoleHelper.WriteWarning("Invalid selection.");
            return null;
        }
        #endregion Private Methods
    }
}