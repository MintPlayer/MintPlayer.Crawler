using Newtonsoft.Json;

namespace MintPlayer.Crawler.Request.LdJson
{
    public class MusicGroup : Subject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
