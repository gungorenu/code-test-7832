using System;

namespace CodeTest
{
    /// <summary>
    /// Console handler, uses System.Console
    /// </summary>
    /// <remarks>Comments are on interface
    /// This class is copied from another codetest</remarks>
    internal class Console : IConsole
    {
        private readonly object _syncObject;

        public Console()
        {
            _syncObject = new object();
        }

        public void Log(ConsoleColor color, string format, params object[] args)
        {
            lock (_syncObject)
            {
                var defaultColor = System.Console.ForegroundColor;
                try
                {
                    System.Console.ForegroundColor = color;
                    System.Console.Write(format, args);
                }
                finally
                {
                    System.Console.ForegroundColor = defaultColor;
                }
            }
        }

        public void Clear()
        {
            lock (_syncObject)
            {
                System.Console.Clear();
            }
        }

        public ConsoleKeyInfo ReadKey()
        {
            lock (_syncObject)
            {
                return System.Console.ReadKey();
            }
        }

        public string ReadValue(string message)
        {
            lock (_syncObject)
            {
                System.Console.Write(message);
                return System.Console.ReadLine();
            }
        }

        public string ReadEnumValue<T>(string message) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("This function requires an Enum type");

            return ReadEnumValue(message, typeof(T));
        }

        public string ReadEnumValue(string message, Type enumType)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException("This function requires an Enum type");

            lock (_syncObject)
            {
                do
                {
                    System.Console.Write(message + "\n");

                    var enums = Enum.GetNames(enumType);

                    for (int i = 0; i < enums.Length; i++)
                    {
                        System.Console.Write("[ {0} ] {1}\n", i + 1, enums[i]);
                    }

                    string value = System.Console.ReadLine();
                    int selection = 0;
                    if (!int.TryParse(value, out selection))
                        continue;
                    if (selection < 1 || selection > enums.Length)
                        continue;
                    return enums[selection - 1];
                }
                while (true);
            }
        }

        public void Error(string format, params object[] args)
        {
            Log(ConsoleColor.Red, format, args);
        }

        public void Exception(Exception ex)
        {
            lock (_syncObject)
            {
                Log(ConsoleColor.Red, "Error Occured: {0}\n", ex.Message);
                Log(ConsoleColor.Red, "Error Stack: {0}\n", ex.StackTrace);
            }
        }
    }
}
