using Newtonsoft.Json;
#nullable enable
namespace WeiPo.Services.Models
{
    public class WeiboResponse<T> where T: class
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T?  Data { get; set; }

        [JsonProperty("ok", NullValueHandling = NullValueHandling.Ignore)]
        public long?  Ok { get; set; }

        [JsonProperty("http_code", NullValueHandling = NullValueHandling.Ignore)]
        public long?  HttpCode { get; set; }
    }
}