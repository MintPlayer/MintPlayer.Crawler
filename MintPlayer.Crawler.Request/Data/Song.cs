using System;
using System.Collections.Generic;

namespace MintPlayer.Crawler.Request.Data
{
    public class Song : Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public Artist PrimaryArtist { get; set; }
        public List<Artist> FeaturedArtists { get; set; }
        public string Url { get; set; }
        public string Lyrics { get; set; }
        public List<Medium> Media { get; set; }
    }
}
