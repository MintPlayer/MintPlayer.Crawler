using System;
using System.Collections.Generic;
using System.Text;

namespace MintPlayer.Crawler.Request.Data
{
    public class Artist : Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }

        public List<Song> Songs { get; set; }
    }
}
