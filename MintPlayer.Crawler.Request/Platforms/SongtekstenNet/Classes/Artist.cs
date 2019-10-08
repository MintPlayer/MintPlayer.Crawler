namespace MintPlayer.Crawler.Request.Platforms.SongtekstenNet.Classes
{
    internal class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public Data.Artist ToDto()
        {
            return new Data.Artist
            {
                Id = Id,
                Name = Name,
                Url = Url
            };
        }
    }
}
