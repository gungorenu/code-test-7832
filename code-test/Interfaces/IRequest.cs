namespace CodeTest
{

    /// <summary>
    /// Web client request instance
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Adds a header to request
        /// </summary>
        /// <param name="name">Header name</param>
        /// <param name="value">Header value</param>
        /// <returns>Request instance back</returns>
        IRequest AddHeader(string name, string value);
        /// <summary>
        /// Adds a parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Request instance back</returns>
        IRequest AddParameter(string name, object value);

        /// <summary>
        /// Adds a parameter into Body
        /// </summary>
        /// <param name="value">Body content value</param>
        /// <returns>Request instance back</returns>
        IRequest AddJsonBodyParameter(object value);

        /// <summary>
        /// Executes the request and returns the response data
        /// </summary>
        /// <typeparam name="T">Response data type</typeparam>
        /// <returns>Response object</returns>
        T Execute<T>() where T : ApiResponseObject, new();
    }
}
