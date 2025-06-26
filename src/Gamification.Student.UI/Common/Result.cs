using System.Text.Json.Serialization;

namespace Common
{
    public class Result
    {

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("succeeded")]
        public bool Succeeded { get; set; }
    }

    public class Result<T> : Result
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}