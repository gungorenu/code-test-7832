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
        /// Adds a parameter into Query (Url Segment)
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Request instance back</returns>
        IRequest AddQueryParameter(string name, object value);

        /// <summary>
        /// Adds a file content to request (to upload a file for example)
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="fileName">File name (not path)</param>
        /// <param name="contentType">Content type (extension)</param>
        /// <param name="value">File data (warning! no size limit)</param>
        /// <returns>Request instance back</returns>
        IRequest AddFile(string name, string fileName, string contentType, byte[] value);

        /// <summary>
        /// Executes the request and returns the response data
        /// </summary>
        /// <typeparam name="T">Response data type</typeparam>
        /// <returns>Response object</returns>
        T Execute<T>() where T : ApiResponseObject, new();

        /// <summary>
        /// Executes the request and returns the response data
        /// </summary>
        /// <typeparam name="T">Response data type</typeparam>
        /// <param name="responseContent">Response back in parameter</param>
        /// <returns>Response object</returns>
        T Execute<T>(out string responseContent) where T : ApiResponseObject, new();
    }
}
