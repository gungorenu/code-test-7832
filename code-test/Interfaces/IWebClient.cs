namespace CodeTest
{

    /// <summary>
    /// Web client for main API caller handler
    /// </summary>
    public interface IWebClient
    {
        /// <summary>
        /// Initiates a GET call
        /// </summary>
        /// <param name="api">Api uri</param>
        /// <returns>Request instance</returns>
        IRequest GET(string api);

        /// <summary>
        /// Initiates a POST call
        /// </summary>
        /// <param name="api">Api uri</param>
        /// <returns>Request instance</returns>
        IRequest POST(string api);

        /// <summary>
        /// Initiates a DELETE call
        /// </summary>
        /// <param name="api">Api uri</param>
        /// <returns>Request instance</returns>
        IRequest DELETE(string api);

        /// <summary>
        /// Initiates a PUT call
        /// </summary>
        /// <param name="api">Api uri</param>
        /// <returns>Request instance</returns>
        IRequest PUT(string api);
    }
}
