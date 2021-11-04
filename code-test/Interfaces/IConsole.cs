using Newtonsoft.Json.Linq;
using System;

namespace CodeTest
{
    /// <summary>
    /// Console handler
    /// </summary>
    internal interface IConsole
    {
        /// <summary>
        /// Logs into console
        /// </summary>
        /// <param name="color">Console text color</param>
        /// <param name="format">Console message</param>
        /// <param name="args">Console message arguments</param>
        void Log(ConsoleColor color, string format, params object[] args);

        /// <summary>
        /// Logs error into console
        /// </summary>
        /// <param name="format">Console message</param>
        /// <param name="args">Console message arguments</param>
        void Error(string format, params object[] args);

        /// <summary>
        /// Logs exception into console
        /// </summary>
        /// <param name="ex">Exception</param>
        void Exception(Exception ex);

        /// <summary>
        /// Clears console
        /// </summary>
        void Clear();

        /// <summary>
        /// Reads a key, and returns read key info
        /// </summary>
        /// <returns>Key code pressed</returns>
        ConsoleKeyInfo ReadKey();

        /// <summary>
        /// Reads a string value from console
        /// </summary>
        /// <param name="message">Message as description</param>
        /// <returns>Read string value</returns>
        string ReadValue(string message);

        /// <summary>
        /// Reads an enum value from console
        /// </summary>
        /// <param name="message">Message as description</param>
        /// <returns>Read string value</returns>
        string ReadEnumValue<T>(string message) where T : struct, IConvertible;


        /// <summary>
        /// Reads a flags type enum value from console
        /// </summary>
        /// <param name="message">Message as description</param>
        /// <param name="enum">Enum value itself</param>
        /// <param name="enumType">Enum type (must be int based)</param>
        /// <returns>Read string value</returns>
        int ReadFlagsEnumValue(string message, Type enumType, int @enum);

        /// <summary>
        /// Prints json object to console
        /// </summary>
        /// <param name="obj">Json object</param>
        /// <param name="indent">indentation for recursiveness</param>
        void Print(JObject obj, int indent = 0);

        /// <summary>
        /// Re-Reads given dynamic object from console
        /// </summary>
        /// <param name="metadata">Dynamic object (for understanding data structure)</param>
        /// <returns>Json object back for simple serialization</returns>
        JObject InputObject(object metadata);
    }
}
