using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MintPlayer.Crawler.Request.Data;

namespace MintPlayer.Crawler.Request.Platforms.LyricsCom
{
    internal class LyricsComFetcher : Fetcher
    {
        internal static Regex UrlFormat
        {
            get => new Regex(@"https\:\/\/www\.lyrics\.com\/.+");
        }

        public override async Task<Subject> Fetch(HttpClient httpClient, bool trimTrash)
        {
            var html = await SendRequest(httpClient);
            var youtube = ExtractYoutubeUrl(html);

            var result = new Song
            {
                Id = ExtractId(),
                Url = Url,
                Title = ExtractTitle(html),
                PrimaryArtist = ExtractArtist(html),
                Lyrics = ExtractLyrics(html, true),
                Media = new List<Medium>()
            };

            if (youtube != null)
                result.Media.Add(new Medium { Type = eMediumType.YouTube, Value = youtube });

            return result;
        }

        private int ExtractId()
        {
            return Convert.ToInt32(Url.Split('/').Last());
        }

        private string ExtractTitle(string html)
        {
            var h1Regex = new Regex(@"\<h1.*?\>(?<title>.*?)\<\/h1\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var h1Match = h1Regex.Match(html);
            if (!h1Match.Success) throw new Exception("No H1 tag found");

            return h1Match.Groups["title"].Value;
        }

        private string ExtractLyrics(string html, bool trimTrash)
        {
            var preRegex = new Regex(@"\<pre.*?\>(?<body>.*?)\<\/pre\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var preMatch = preRegex.Match(html);
            if (!preMatch.Success) throw new Exception("No pre tag found");

            var stripARegex = new Regex(@"\<a.*?\>|\<\/a\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var stripped = stripARegex.Replace(preMatch.Groups["body"].Value, "");
            var whitespaces_stripped = stripped.Replace("\r\n", Environment.NewLine).Trim();

            return whitespaces_stripped;
        }

        private string ExtractYoutubeUrl(string html)
        {
            var idRegex = new Regex(@"\<div class=\""youtube-player\"" data-id=\""(?<id>.*?)\""\>\<\/div\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var idMatch = idRegex.Match(html);
            if (!idMatch.Success) return null;

            var id = idMatch.Groups["id"].Value;

            return $"http://www.youtube.com/watch?v={id}";
        }

        private Artist ExtractArtist(string html)
        {
            var h3Regex = new Regex(@"\<h3 class=\""lyric-artist\""\>\<a href=\""(?<url>.*?)\""\>(?<name>.*?)\<\/a\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var h3Match = h3Regex.Match(html);
            if (!h3Match.Success) return null;

            return new Artist
            {
                Id = Convert.ToInt32(h3Match.Groups["url"].Value.Split('/').Last()),
                Name = h3Match.Groups["name"].Value,
                Url = "https://www.lyrics.com/" + h3Match.Groups["url"].Value
            };
        }
    }
}
