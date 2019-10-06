using MintPlayer.Crawler.Request.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MintPlayer.Crawler.Request.Platforms
{
    internal class GeniusFetcher : Fetcher
    {
        internal static Regex UrlFormat
        {
            get
            {
                return new Regex(@"https\:\/\/genius.com\/.+");
            }
        }

        public override async Task<Subject> Fetch(HttpClient httpClient, string url)
        {
            var subject = await base.Fetch(httpClient, url);

            switch (subject.Type)
            {
                case "MusicRecording":
                    var recording = JsonConvert.DeserializeObject<MusicRecording>(ldJsonMatch.Value);

                    return recording;
                case "MusicGroup":
                    var group = JsonConvert.DeserializeObject<MusicGroup>(ldJsonMatch.Value);
                    return group;
                default:
                    throw new Exception($"Subject type {subject.Type} not recognized");
            }
        }
    }
}
