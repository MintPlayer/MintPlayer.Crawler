using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MintPlayer.Crawler.Request.Platforms.Genius.Classes
{
    internal class AlbumData
    {
        [JsonProperty("album")]
        public Album Album { get; set; }

        [JsonProperty("album_appearances")]
        public List<AlbumAppearance> Tracks { get; set; }

        public Data.Album ToDto()
        {
            return new Data.Album
            {
                Id = Album.Id,
                Name = Album.Name,
                ReleaseDate = Album.ReleaseDate,
                CoverArtUrl = Album.CoverArtUrl,
                Artist = Album.Artist.ToDto(),
                Url = Album.Url,
                Tracks = Tracks
                    .OrderBy(t => t.TrackNumber)
                    .Select(t => t.Song.ToDto())
                    .ToList()
            };
        }
    }
}
