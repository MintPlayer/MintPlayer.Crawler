using System;
using System.Collections.Generic;
using System.Text;

namespace MintPlayer.Crawler.Request.Platforms.SongtekstenNet.Classes
{
    internal class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public Data.Song ToDto()
        {
            return new Data.Song
            {
                Id = Id,
                Title = Title,
                Url = Url
            };
        }
    }
}
