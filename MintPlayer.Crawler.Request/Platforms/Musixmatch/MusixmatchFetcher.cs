using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = new Data.Song
            {
                Lyrics = ExtractLyrics(html, true)
            };
            return result;
        }

        private string ExtractLyrics(string html, bool trimTrash)
        {
            var spanRegex = new Regex(@"(?<=\<span class=\""lyrics__content__ok\""\>).*?(?=\<\/span\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var spanMatches = spanRegex.Matches(html);
            if (spanMatches.Count == 0) throw new Exception("span tag not found");

            var matches = new Match[spanMatches.Count];
            spanMatches.CopyTo(matches, 0);

            return string.Join("\r\n\r\n", matches.Select(m => m.Value));
        }
    }
}
