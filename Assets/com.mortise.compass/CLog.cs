using System;

namespace MortiseFrame.Compass {

    public static class CLog {
        public static Action<string> Log = Console.WriteLine;
        public static Action<string> LogWarning = (msg) => Console.WriteLine($"WARNING: {msg}");
        public static Action<string> LogError = (msg) => Console.Error.WriteLine($"ERROR: {msg}");
    }

}