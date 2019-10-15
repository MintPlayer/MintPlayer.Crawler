using Newtonsoft.Json;

namespace MintPlayer.Crawler.Request.Platforms.Musixmatch.Classes
{
    /// <summary>This class is only used to read the @type field from a json string.</summary>
    internal class Subject
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
    }
}
