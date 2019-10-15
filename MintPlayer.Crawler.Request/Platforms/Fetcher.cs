using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MintPlayer.Crawler.Request.Platforms
{
    internal abstract class Fetcher
    {
        public static Fetcher ByUrl(string url)
        {
            if (Genius.GeniusFetcher.UrlFormat.IsMatch(url))
            {
                return new Genius.GeniusFetcher { Url = url };
            }
            else if (Musixmatch.MusixmatchFetcher.UrlFormat.IsMatch(url))
            {
                return new Musixmatch.MusixmatchFetcher { Url = url };
            }
            else if (SongtekstenNet.SongtekstenNetFetcher.UrlFormat.IsMatch(url))
            {
                return new SongtekstenNet.SongtekstenNetFetcher { Url = url };
            }
            else if (LyricsCom.LyricsComFetcher.UrlFormat.IsMatch(url))
            {
                return new LyricsCom.LyricsComFetcher { Url = url };
            }
            else
            {
                throw new Exception("No fetcher found for this url");
            }
        }

        protected string Url { get; private set; }
        protected string Html { get; set; }

        protected async Task<string> SendRequest(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(Url);
            Html = await response.Content.ReadAsStringAsync();
            return Html;
        }

        //protected virtual async Task<string> ReadLdJson()
        //{
        //    var ldJsonRegex = new Regex(@"(?<=\<script type=\""application\/ld\+json\"">).*?(?=\<\/script\>)", RegexOptions.Singleline | RegexOptions.Multiline);
        //    var ldJsonMatch = ldJsonRegex.Match(Html);

        //    if (!ldJsonMatch.Success) throw new Exception("No LD+json tag found");

        //    return ldJsonMatch.Value;
        //}

        public abstract Task<Data.Subject> Fetch(HttpClient httpClient, bool trimTrash);
    }
}
