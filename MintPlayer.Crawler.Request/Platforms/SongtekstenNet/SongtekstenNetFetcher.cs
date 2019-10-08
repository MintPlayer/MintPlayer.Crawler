using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MintPlayer.Crawler.Request.Data;

namespace MintPlayer.Crawler.Request.Platforms.SongtekstenNet
{
    internal class SongtekstenNetFetcher : Fetcher
    {
        internal static Regex UrlFormat
        {
            get
            {
                return new Regex(@"https\:\/\/songteksten\.net\/.+");
            }
        }

        public override async Task<Subject> Fetch(HttpClient httpClient, bool trimTrash)
        {
            var html = await SendRequest(httpClient);
            var splitted = Url.Split('/');

            if (Url.StartsWith("https://songteksten.net/lyric/"))
            {
                return new Song
                {
                    Url = Url,
                    Id = Convert.ToInt32(splitted[splitted.Length - 3]),
                    Lyrics = ExtractSongLyrics(html, true),
                    Title = ExtractSongTitle(html),
                    Artists = ExtractSongArtists(html).Select(a => a.ToDto()).ToList()
                };
            }
            else if (Url.StartsWith("https://songteksten.net/artist/"))
            {
                return new Artist
                {
                    Url = Url,
                    Id = Convert.ToInt32(splitted[splitted.Length - 2]),
                    Name = ExtractArtistName(html),
                    Songs = ExtractArtistSongs(html).Select(s => s.ToDto()).ToList()
                };
            }
            else if (Url.StartsWith("https://songteksten.net/albums/"))
            {
                // Album
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception();
            }
        }

        #region Song
        private string ExtractSongLyrics(string html, bool trimTrash)
        {
            var regex = new Regex(@"(?<=\<\/h1\>).*?(?=\<div)", RegexOptions.Singleline | RegexOptions.Multiline);
            var match = regex.Match(html);
            if (!match.Success) throw new Exception("No tag found");

            var whitespaces_stripped = match.Value.Replace("\r", "").Replace("\n", "").Replace("<br />", Environment.NewLine);

            return whitespaces_stripped;
        }

        private string ExtractSongTitle(string html)
        {
            var breadcrumbRegex = new Regex(@"(?<=\<ol class=\""breadcrumb\""\>).*?(?=\<\/ol\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var breadcrumbMatch = breadcrumbRegex.Match(html);
            if (!breadcrumbMatch.Success) throw new Exception("No breadcrumb found");
            
            var bc = breadcrumbMatch.Value;
            var titleregex = new Regex(@"(?<=\<li\>).*?(?=\<\/li\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var titleMatches = titleregex.Matches(bc);
            if (titleMatches.Count == 0) throw new Exception("No title found");

            return titleMatches[titleMatches.Count - 1].Value;
        }

        private List<Classes.Artist> ExtractSongArtists(string html)
        {
            var regexUl = new Regex(@"\<h3\>Artiesten\<\/h3\>\s*\<ul.*?\>\s*(.*?)\s*\<\/ul\>");
            var matchUl = regexUl.Match(html);
            if (!matchUl.Success) throw new Exception("No UL found");

            var regexLi = new Regex(@"\<li.*?\>.*?\<a href\=\""(?<url>.*?)\""\>(?<name>.*?)\<\/a\>\<\/li\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var artist_matches = regexLi.Matches(matchUl.Value);
            var arr_matches = new Match[artist_matches.Count];
            artist_matches.CopyTo(arr_matches, 0);


            return arr_matches.Select(m => {
                var url_split = m.Groups["url"].Value.Split('/');
                return new Classes.Artist
                {
                    Id = Convert.ToInt32(url_split[url_split.Length - 2]),
                    Url = m.Groups["url"].Value,
                    Name = m.Groups["name"].Value
                };
            }).ToList();
        }
        #endregion
        #region Artist
        private string ExtractArtistName(string html)
        {
            var breadcrumbRegex = new Regex(@"(?<=\<ol class=\""breadcrumb\""\>).*?(?=\<\/ol\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var breadcrumbMatch = breadcrumbRegex.Match(html);
            if (!breadcrumbMatch.Success) throw new Exception("No breadcrumb found");

            var bc = breadcrumbMatch.Value;
            var liRegex = new Regex(@"(?<=\<li\>).*?(?=\<\/li\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var liMatches = liRegex.Matches(bc);
            if (liMatches.Count == 0) throw new Exception("No title found");

            var a = liMatches[liMatches.Count - 2].Value;
            var aRegex = new Regex(@"(?<=\<a.*?\>).*?(?=\<\/a\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var aMatch = aRegex.Match(a);
            if (!aMatch.Success) throw new Exception("No a tag found");

            return aMatch.Value;
        }
        private List<Classes.Song> ExtractArtistSongs(string html)
        {
            var h1Regex = new Regex(@"\<\/h1\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var main = h1Regex.Split(html)[1];

            var ulRegex = new Regex(@"\<ul class\=\""list-unstyled\""\>(?<lis>.*?)\<\/ul\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var songsMatch = ulRegex.Match(main).Groups["lis"];

            var songRegex = new Regex(@"\<a href=\""(?<url>.*?)\"">(?<title>.*?)\<\/a\>");
            var songMatches = songRegex.Matches(songsMatch.Value);
            var arrSongsMatches = new Match[songMatches.Count];
            songMatches.CopyTo(arrSongsMatches, 0);

            return arrSongsMatches.Select(m => {
                var parts = m.Groups["url"].Value.Split('/');
                return new Classes.Song
                {
                    Id = Convert.ToInt32(parts[parts.Length - 3]),
                    Title = m.Groups["title"].Value,
                    Url = m.Groups["url"].Value
                };
            }).ToList();
        }
        #endregion
    }
}
