using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MintPlayer.Crawler.Request.Data;

namespace MintPlayer.Crawler.Request.Platforms.Musixmatch
{
    internal class MusixmatchFetcher : Fetcher
    {
        internal static Regex UrlFormat
        {
            get
            {
                return new Regex(@"https\:\/\/(www\.){0,1}musixmatch.com\/.+");
            }
        }

        public override async Task<Subject> Fetch(HttpClient httpClient, bool trimTrash)
        {
            var html = await SendRequest(httpClient);
            return null;
        }
    }
}
