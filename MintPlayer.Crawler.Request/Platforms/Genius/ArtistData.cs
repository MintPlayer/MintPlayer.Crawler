using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MintPlayer.Crawler.Request.Platforms.Genius
{
    internal class ArtistData
    {
        [JsonProperty("artist")]
        public Artist Artist { get; set; }
    }
}
