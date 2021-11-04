using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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

        public int ReadFlagsEnumValue(string message, Type enumType, int @enum)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException("This function requires an Enum type");

            lock (_syncObject)
            {
                do
                {
                    System.Console.Write(message + "\n");

                    var enums = Enum.GetNames(enumType);
                    List<string> options = new List<string>();
                    for (int i = 0; i < enums.Length; i++)
                    {
                        int flag = (int)Enum.Parse(enumType, enums[i]);
                        if ( (flag & @enum) == flag)
                        {
                            options.Add(enums[i]);
                            System.Console.Write("[ {0} ] {1}\n", options.Count, enums[i]);
                        }
                    }

                    string value = System.Console.ReadLine();
                    int selection = 0;
                    if (!int.TryParse(value, out selection))
                        continue;

                    if (selection < 0 || selection > options.Count)
                        continue;

                    return (int)Enum.Parse(enumType, options[selection - 1]);
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

        public void Print(JObject obj, int indent = 0)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            lock (_syncObject)
            {
                string indentation = new string(' ', indent * 3);
                foreach (var prop in obj.Properties())
                {
                    if (prop.Value.Type == JTokenType.Array)
                    {
                        Log(ConsoleColor.Green, $"{indentation}{prop.Name} [] :\n");
                        Print(prop.Value as JArray, indent + 1);
                    }
                    else if (prop.Value.Type == JTokenType.Object)
                    {
                        Log(ConsoleColor.Green, $"{indentation}{{{{\n");
                        Print(prop.Value as JObject, indent + 1);
                        Log(ConsoleColor.Green, $"{indentation}}}}}\n");
                    }
                    else
                    {
                        Log(ConsoleColor.Green, $"{indentation}{prop.Name}: ");
                        Log(ConsoleColor.Gray, $"{prop.Value}\n", prop.Value);
                    }
                }
            }
        }

        public void Print(JArray arr, int indent = 0)
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));

            lock (_syncObject)
            {
                string indentation = new string(' ', indent * 3);
                foreach (var ele in arr)
                {
                    if (ele.Type == JTokenType.Array)
                    {
                        Print(ele as JArray, indent + 1);
                    }
                    else if (ele.Type == JTokenType.Object)
                    {
                        Log(ConsoleColor.Gray, $"{indentation}{{{{\n");
                        Print(ele as JObject, indent + 1);
                        Log(ConsoleColor.Gray, $"{indentation}}}}}\n");
                    }
                    else
                    {
                        Log(ConsoleColor.Gray, $"{indentation}{ele}");
                    }
                }
            }
        }

        public JObject InputObject(object metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            lock (_syncObject)
            {
                var jsonObj = new JObject();
                foreach (var prop in metadata.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                {
                    string value = "";
                    if (prop.PropertyType.Equals(typeof(string)))
                    {
                        value = ReadValue($"Enter text for '{prop.Name}' and [Enter]: ");
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        value = ReadEnumValue($"Enter one of the options below for '{prop.Name}' and [Enter]: ", prop.PropertyType);
                    }

                    jsonObj.Add(prop.Name, JToken.FromObject(value));
                }

                return jsonObj;
            }
        }
    }
}
