using Newtonsoft.Json.Linq;
using System;

namespace CodeTest
{
    /// <summary>
    /// since I do not want to create many classes for models, I use such a helper class to produce properties dynamically
    /// Performance would be slower but not a big deal for the assignment I hope
    /// </summary>
    public static class ModelHelper
    {

        /// <summary>
        /// Creates a model Json object with application key + password pair
        /// </summary>
        /// <param name="appKey">Application key</param>
        /// <param name="password">Password</param>
        /// <returns>JsonObject back</returns>
        public static JObject ApplicationBaseModel(string appKey, string password)
        {
            var result = new JObject();
            result.Add("applicationKey", appKey);
            result.Add("password", password);
            return result;
        }
    }
}
