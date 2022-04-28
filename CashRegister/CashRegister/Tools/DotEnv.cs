using System;
using System.IO;
using System.Diagnostics;

namespace CashRegister.Tools
{
    // https://dusted.codes/dotenv-in-dotnet
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            Debug.WriteLine("loading " + filePath);
            Debug.WriteLine("searching on " + Environment.CurrentDirectory);
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            Debug.WriteLine("\nReading .env file...");
            foreach (var line in File.ReadAllLines(filePath))
            {
                /* works on CashRegisterServer, not here, .netCore version ?
                var parts = line.Split(
                    '=',
                    2,
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                */

                var parts = line.Split(
                    new char[] { '=' },
                    2,
                    StringSplitOptions.RemoveEmptyEntries);                
                
                if (parts.Length != 2)
                {
                    Debug.WriteLine("invalid line: " + line);
                    continue;
                }
                Debug.WriteLine("setting " + parts[0] + " to " + parts[1]);
                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
            Console.WriteLine("Finished reading\n");
        }
    }
}
