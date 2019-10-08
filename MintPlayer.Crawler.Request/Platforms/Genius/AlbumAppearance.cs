using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MintPlayer.Crawler.Request.Platforms.Genius
{
    internal class AlbumAppearance
    {
        [JsonProperty("track_number")]
        public int? TrackNumber { get; set; }

        [JsonProperty("song")]
        public Song Song { get; set; }
    }
}
