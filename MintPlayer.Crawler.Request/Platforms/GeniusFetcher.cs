using MintPlayer.Crawler.Request.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override async Task<Subject> Fetch(HttpClient httpClient)
        {
            var html = await SendRequest(httpClient);
            var ld_json = await ReadLdJson();

            var subject = JsonConvert.DeserializeObject<Subject>(ld_json);


            switch (subject.Type)
            {
                case "MusicRecording":
                    var recording = JsonConvert.DeserializeObject<MusicRecording>(ld_json);

                    // Read lyrics
                    recording.Lyrics = ExtractLyrics(Html);

                    return recording;
                case "MusicGroup":
                    var group = JsonConvert.DeserializeObject<MusicGroup>(ld_json);
                    return group;
                default:
                    throw new Exception($"Subject type {subject.Type} not recognized");
            }
        }

        private string ExtractLyrics(string html)
        {
            var lyricsRegex = new Regex(@"(?<=\<div class=\""lyrics\""\>).*?(?=\<\/div\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var lyricsMatch = lyricsRegex.Match(html);
            if (!lyricsMatch.Success) throw new Exception("No lyrics tag found");

            var pRegex = new Regex(@"(?<=\<p\>).*?(?=\<\/p\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var pMatch = pRegex.Match(lyricsMatch.Value);
            if (!pMatch.Success) throw new Exception("No P tag found");

            var stripARegex = new Regex(@"\<a.*?\>|\<\/a\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var stripped = stripARegex.Replace(pMatch.Value, "");

            return stripped.Replace("\r", "").Replace("\n", "").Replace("<br>", Environment.NewLine);
        }
    }
}
