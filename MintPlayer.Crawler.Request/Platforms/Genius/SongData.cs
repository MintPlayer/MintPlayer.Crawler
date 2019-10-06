using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MintPlayer.Crawler.Request.Platforms.Genius
{
    internal class SongData
    {
        [JsonProperty("song")]
        public Song Song { get; set; }
    }
}
