using MintPlayer.Crawler.Request.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MintPlayer.Crawler.Request.Platforms.Genius
{
    internal class Song
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("apple_music_id")]
        public string AppleMusicId { get; set; }

        [JsonProperty("soundcloud_url")]
        public string SoundCloudUrl { get; set; }

        [JsonProperty("spotify_uuid")]
        public string SpotifyUuid { get; set; }

        [JsonProperty("release_date")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("primary_artist")]
        public Artist PrimaryArtist { get; set; }

        [JsonProperty("featured_artists")]
        public List<Artist> FeaturedArtists { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonIgnore]
        public string Lyrics { get; set; }

        public Data.Song ToDto()
        {
            return new Data.Song
            {
                Title = Title,
                ReleaseDate = ReleaseDate,
                PrimaryArtist = PrimaryArtist.ToDto(),
                FeaturedArtists = FeaturedArtists == null ? null : FeaturedArtists.Select(a => a.ToDto()).ToList(),
                Url = Url,
                Lyrics = Lyrics
            };
        }
    }
}
