using Newtonsoft.Json;

namespace CodeTest
{
    public class ApiResponseObject
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class AppKeyApiResponseObject : ApiResponseObject
    {
        [JsonProperty("applicationKey")]
        public string ApplicationKey { get; set; }
    }


    public class UserKeyApiResponseObject : ApiResponseObject
    {
        [JsonProperty("userKey")]
        public string UserKey { get; set; }
    }
}
