using Newtonsoft.Json;

namespace MintPlayer.Crawler.Request.Data
{
    public class Subject
    {
        internal Subject()
        {
        }

        [JsonProperty("@type")]
        internal string Type { get; set; }
    }
}
