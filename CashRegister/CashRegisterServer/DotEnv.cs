namespace CashRegisterServer
{
    // https://dusted.codes/dotenv-in-dotnet
    using System;
    using System.IO;
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;
            
            Console.WriteLine("\nReading .env file...");
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    2,
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (parts.Length != 2)
                {
                    Console.WriteLine("invalid line: " + line);
                    continue;
                }

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
            Console.WriteLine("Finished reading\n");
        }
    }
}
