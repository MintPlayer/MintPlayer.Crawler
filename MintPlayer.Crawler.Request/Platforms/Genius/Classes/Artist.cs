using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MintPlayer.Crawler.Request.Platforms.Genius.Classes
{
    internal class Artist
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public Data.Artist ToDto()
        {
            return new Data.Artist
            {
                Id = Id,
                Name = Name,
                ImageUrl = ImageUrl,
                Url = Url
            };
        }
    }
}
