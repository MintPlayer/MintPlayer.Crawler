﻿using System;
using MintPlayer.Crawler.Request;

namespace MintPlayer.Crawler.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var crawler = new MintPlayerCrawler();
            //var res = crawler.GetByUrl("https://genius.com/Daft-punk-get-lucky-lyrics", true).Result;
            //var res0 = crawler.GetByUrl("https://genius.com/artists/Daft-punk", true).Result;
            //var res1 = crawler.GetByUrl("https://genius.com/albums/Daft-punk/Random-access-memories", true).Result;

            //var res2 = crawler.GetByUrl("https://www.musixmatch.com/lyrics/Daft-Punk-feat-Nile-Rodgers-Pharrell-Williams/Get-lucky", true).Result;

            //var res3 = crawler.GetByUrl("https://songteksten.net/lyric/97/96025/daft-punk/get-lucky.html", true).Result;
            //var res4 = crawler.GetByUrl("https://songteksten.net/artist/lyrics/97/daft-punk.html", true).Result;
            var res5 = crawler.GetByUrl("https://genius.com/Avicii-sunset-jesus-lyrics", true).Result;
            //var res6 = crawler.GetByUrl("https://genius.com/artists/Avicii", true).Result;


            //var res = crawler.GetByUrl("https://genius.com/France-gall-ella-elle-la-lyrics", true).Result;
            //var res = crawler.GetByUrl("https://songteksten.net/lyric/1818/11910/france-gall/ella-elle-la.html", true).Result;
            //var res = crawler.GetByUrl("https://www.musixmatch.com/lyrics/France-Gall/Ella-Elle-L-a", true).Result;

            var res3 = crawler.GetByUrl("https://www.lyrics.com/lyric/29531449", true).Result;
        
        }
    }
}
