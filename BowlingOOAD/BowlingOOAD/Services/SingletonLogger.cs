using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingOOAD.Services
{
    public sealed class SingletonLogger
    {
        private static readonly SingletonLogger _instance = new();
        private SingletonLogger() { }
        public static SingletonLogger Instance => _instance;

        public void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[LOG]: {message}");
            Console.ResetColor();
        }

    }
}
