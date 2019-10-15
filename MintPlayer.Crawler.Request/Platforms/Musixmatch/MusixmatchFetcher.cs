using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MintPlayer.Crawler.Request.Data;
using Newtonsoft.Json;

namespace MintPlayer.Crawler.Request.Platforms.Musixmatch
{
    /// <summary>This fetcher can only extract the title and lyrics from an URL.</summary>
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
            var ld_json = ReadLdJson(html);
            var subject = JsonConvert.DeserializeObject<Classes.Subject>(ld_json);

            switch (subject.Type)
            {
                case "MusicRecording":
                    {
                        var song = JsonConvert.DeserializeObject<Classes.Song>(ld_json);
                        var result = new Song
                        {
                            Lyrics = ExtractLyrics(html, true),
                            Title = song.Title,
                            Url = Url
                        };
                        return result;
                    }
                default:
                    throw new NotImplementedException();
            }
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

        private string ReadLdJson(string html)
        {
            var ldJsonRegex = new Regex(@"\<script data-react-helmet=""true"" type=\""application\/ld\+json\"".*?\>(?<body>.*?)\<\/script\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var ldJsonMatch = ldJsonRegex.Match(html);
            if (!ldJsonMatch.Success) throw new Exception("No ld+json tag found");

            return ldJsonMatch.Groups["body"].Value;
        }
    }
}
