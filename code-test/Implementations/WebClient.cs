using RestSharp;
using System;

namespace CodeTest
{
    /// <summary>
    /// Web client implementation, helper class
    /// </summary>
    internal class WebClient : IWebClient
    {
        private class JsonNetSerializer : RestSharp.Serialization.IRestSerializer
        {
            public string Serialize(object obj) =>
                Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            public string Serialize(Parameter parameter) =>
                Newtonsoft.Json.JsonConvert.SerializeObject(parameter.Value);

            public T Deserialize<T>(RestSharp.IRestResponse response) =>
                Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);

            public string[] SupportedContentTypes { get; } = {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json" };

            public string ContentType { get; set; } = "application/json";

            public RestSharp.DataFormat DataFormat { get; } = RestSharp.DataFormat.Json;
        }

        internal class InternalRequest : IRequest
        {
            private readonly WebClient _client;

            internal IRestRequest Request { get; private set; }

            public InternalRequest(IRestRequest request, WebClient client)
            {
                Request = request;
                _client = client;
            }

            public IRequest AddHeader(string name, string value)
            {
                Request.AddHeader(name, value);
                return this;
            }

            public IRequest AddParameter(string name, object value)
            {
                Request.AddParameter(name, value);
                return this;
            }

            public IRequest AddQueryParameter(string name, object value)
            {
                Request.AddParameter(name, value, ParameterType.UrlSegment);
                return this;
            }

            public IRequest AddJsonBodyParameter(object value)
            {
                Request.AddJsonBody(value, "application/json");
                return this;
            }

            public IRequest AddFile(string name, string fileName, string contentType, byte[] value)
            {
                Request.AddFileBytes(name, value, fileName, contentType);
                return this;
            }

            public T Execute<T>() where T : ApiResponseObject, new()
            {
                string dummy;
                return Execute<T>(out dummy);
            }

            public T Execute<T>(out string responseContent) where T : ApiResponseObject, new()
            {
                return _client.Execute<T>(this, out responseContent);
            }

        }

        private readonly IConsole _console;
        private RestClient _client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="console">Console instance</param>
        /// <param name="baseUri">Base uri for APIs</param>
        public WebClient(IConsole console, string baseUri)
        {
            if (console == null)
                throw new ArgumentNullException(nameof(console));
            if( string.IsNullOrEmpty(baseUri))
                throw new ArgumentNullException(nameof(baseUri));

            _console = console;
            _client = new RestClient(new Uri(baseUri));
            _client.UseSerializer( () => new JsonNetSerializer());
        }

        public IRequest GET(string api)
        {
            if (string.IsNullOrEmpty(api))
                throw new ArgumentNullException(api);
            return new InternalRequest(new RestRequest(api, Method.GET), this);
        }

        public IRequest POST(string api)
        {
            if (string.IsNullOrEmpty(api))
                throw new ArgumentNullException(api);
            return new InternalRequest(new RestRequest(api, Method.POST), this);
        }

        public IRequest DELETE(string api)
        {
            if (string.IsNullOrEmpty(api))
                throw new ArgumentNullException(api);
            return new InternalRequest(new RestRequest(api, Method.DELETE), this);
        }

        public IRequest PUT(string api)
        {
            if (string.IsNullOrEmpty(api))
                throw new ArgumentNullException(api);
            return new InternalRequest(new RestRequest(api, Method.PUT), this);
        }

        internal T Execute<T>(IRequest apiCall, out string responseContent)
            where T : ApiResponseObject, new()
        {
            responseContent = string.Empty;
            try
            {
                InternalRequest request = (InternalRequest)apiCall;

                IRestResponse<T> result = _client.Execute<T>(request.Request);
                if (result.IsSuccessful)
                {
                    responseContent = result.Content;
                    return result.Data;
                }
                else
                {
                    _console.Error("[{0}] {1} api gave error code {2} and error {3}\n",
                        request.Request.Method,
                        request.Request.Resource,
                        result.StatusCode,
                        result.ErrorMessage);
                    return new T() { Message = string.Format("HTTP {0}", result.StatusCode), Status = false };
                }
            }
            catch (Exception ex)
            {
                _console.Exception(ex);
                return new T() { Message = ex.Message, Status = false };
            }
        }
    }

}
