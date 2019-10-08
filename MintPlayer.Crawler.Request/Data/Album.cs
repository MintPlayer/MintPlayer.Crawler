using System;
using System.Collections.Generic;
using System.Text;

namespace MintPlayer.Crawler.Request.Data
{
    public class Album : Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Artist Artist { get; set; }
        public string CoverArtUrl { get; set; }
        public string Url { get; set; }
        public List<Song> Tracks { get; set; }
    }
}
