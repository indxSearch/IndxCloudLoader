namespace IndxCloudLoader
{
    internal static class ConsoleHelper
    {
        public static void WriteHeader(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n━━━ {message} ━━━");
            Console.ResetColor();
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }

        public static void WriteInfo(string message)
        {
            Console.WriteLine($"  {message}");
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ {message}");
            Console.ResetColor();
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ {message}");
            Console.ResetColor();
        }

        public static void WriteProgress(string message)
        {
            Console.Write($"\r{message}");
        }

        public static void WriteSummary(string title, Dictionary<string, object> items)
        {
            WriteHeader(title);
            foreach (var item in items)
            {
                Console.WriteLine($"  {item.Key}: {item.Value}");
            }
        }
    }
}
