using MintPlayer.Crawler.Request.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MintPlayer.Crawler.Request.Data
{
    public class MusicAlbum : Subject
    {
        [JsonProperty("byArtist")]
        public MusicGroup ByArtist { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("datePublished")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime DatePublished { get; set; }
        [JsonProperty("numTracks")]
        public int NumTracks { get; set; }
    }
}
