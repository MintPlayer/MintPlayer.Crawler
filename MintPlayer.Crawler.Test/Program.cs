using MintPlayer.Crawler.Request;
using System;

namespace MintPlayer.Crawler.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var crawler = new MintPlayerCrawler();
            var res = crawler.GetByUrl("https://genius.com/Daft-punk-get-lucky-lyrics", true).Result;
            var res0 = crawler.GetByUrl("https://genius.com/artists/Daft-punk", true).Result;
        }
    }
}
