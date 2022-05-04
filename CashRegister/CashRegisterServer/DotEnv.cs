using System;
using System.IO;

namespace CashRegisterServer
{
    /**
     * Search if a .env file exists, then load it as environment var.
     * https://dusted.codes/dotenv-in-dotnet
     */
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("No .env");
            }
            
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
