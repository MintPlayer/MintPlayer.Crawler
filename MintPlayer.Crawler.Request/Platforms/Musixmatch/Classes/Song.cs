using Newtonsoft.Json;

namespace MintPlayer.Crawler.Request.Platforms.Musixmatch.Classes
{
    internal class Song
    {
        [JsonProperty("name")]
        public string Title { get; set; }

        public Data.Song ToDto()
        {
            return new Data.Song
            {
                Title = Title
            };
        }
    }
}
