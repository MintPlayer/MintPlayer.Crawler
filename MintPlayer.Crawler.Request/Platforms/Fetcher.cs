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
    internal abstract class Fetcher
    {
        public static Fetcher ByUrl(string url)
        {
            if(GeniusFetcher.UrlFormat.IsMatch(url))
            {
                return new GeniusFetcher();
            }
            else
            {
                throw new Exception("No fetcher found for this url");
            }
        }

        public virtual async Task<string> Fetch(HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var ldJsonRegex = new Regex(@"(?<=\<script type=\""application\/ld\+json\"">).*?(?=\<\/script\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var ldJsonMatch = ldJsonRegex.Match(responseContent);

            if (!ldJsonMatch.Success) throw new Exception("No LD+json tag found");

            var subject = JsonConvert.DeserializeObject<Subject>(ldJsonMatch.Value);

            return subject;
        }
    }
}
