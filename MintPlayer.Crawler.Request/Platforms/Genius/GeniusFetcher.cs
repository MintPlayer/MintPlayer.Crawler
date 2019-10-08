using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MintPlayer.Crawler.Request.Platforms.Genius
{
    internal class GeniusFetcher : Fetcher
    {
        internal static Regex UrlFormat
        {
            get
            {
                return new Regex(@"https\:\/\/genius\.com\/.+");
            }
        }

        public override async Task<Data.Subject> Fetch(HttpClient httpClient, bool trimTrash)
        {
            var html = await SendRequest(httpClient);
            var page_data = await ReadPageData(html);

            var structure = new { page_type = string.Empty };
            var subject = JsonConvert.DeserializeAnonymousType(page_data, structure);

            switch (subject.page_type)
            {
                case "profile":
                    {
                        var data = JsonConvert.DeserializeObject<Classes.ArtistData>(page_data);

                        var songs = new List<Classes.Song>();
                        var albums = new List<Classes.Album>();
                        var page = (int?)1;
                        var songs_structure = new
                        {
                            meta = new
                            {
                                status = 0
                            },
                            response = new
                            {
                                next_page = (int?)0,
                                songs = new List<Classes.Song>()
                            }
                        };
                        var albums_structure = new
                        {
                            meta = new
                            {
                                status = 0
                            },
                            response = new
                            {
                                next_page = (int?)0,
                                albums = new List<Classes.Album>()
                            }
                        };

                        while (true)
                        {
                            var response = await httpClient.GetAsync($"https://genius.com/api/artists/{data.Artist.Id}/songs?per_page=50&page={page}&sort=popularity");
                            var json_songs = await response.Content.ReadAsStringAsync();
                            var data_songs = JsonConvert.DeserializeAnonymousType(json_songs, songs_structure);
                            songs.AddRange(data_songs.response.songs);

                            if ((page = data_songs.response.next_page) == null)
                                break;
                        }

                        page = 1;
                        while (true)
                        {
                            var response = await httpClient.GetAsync($"https://genius.com/api/artists/{data.Artist.Id}/albums?per_page=50&page={page}");
                            var json_albums = await response.Content.ReadAsStringAsync();
                            var data_albums = JsonConvert.DeserializeAnonymousType(json_albums, albums_structure);
                            albums.AddRange(data_albums.response.albums);

                            if ((page = data_albums.response.next_page) == null)
                                break;
                        }

                        data.Songs = songs;
                        data.Albums = albums;

                        return data.ToDto();
                    }
                case "song":
                    {
                        var data = JsonConvert.DeserializeObject<Classes.SongData>(page_data);
                        data.Song.Lyrics = ExtractLyrics(data.LyricsData.Body.Html, trimTrash);
                        return data.Song.ToDto();
                    }
                case "album":
                    {
                        var data = JsonConvert.DeserializeObject<Classes.AlbumData>(page_data);
                        return data.ToDto();
                    }
                default:
                    throw new Exception("Type not recognized");
            }
        }

        public async Task<string> ReadPageData(string html)
        {
            var pageDataRegex = new Regex(@"(?<=\<meta content\=\"")(.*?)(?=\""\sitemprop\=\""page_data\""\>\<\/meta\>)");
           
            var pageData = pageDataRegex.Match(html).Value;
            var fixedPageData = pageData
                .Replace("&quot;", "\"")
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&#39;", "'");

            return fixedPageData;
        }

        private string ExtractLyrics(string pageDataBodyHtml, bool trimTrash)
        {
            var pRegex = new Regex(@"(?<=\<p\>).*?(?=\<\/p\>)", RegexOptions.Singleline | RegexOptions.Multiline);
            var pMatch = pRegex.Match(pageDataBodyHtml);
            if (!pMatch.Success) throw new Exception("No P tag found");

            var stripARegex = new Regex(@"\<a.*?\>|\<\/a\>", RegexOptions.Singleline | RegexOptions.Multiline);
            var stripped = stripARegex.Replace(pMatch.Value, "");
            var whitespaces_stripped = stripped.Replace("\r", "").Replace("\n", "").Replace("<br>", Environment.NewLine);

            if(trimTrash)
            {
                var stripBracketsRegex = new Regex(@"\[.*?\]\r\n", RegexOptions.Singleline | RegexOptions.Multiline);
                var brackets_stripped = stripBracketsRegex.Replace(whitespaces_stripped, "");
                return brackets_stripped;
            }
            else
            {
                return whitespaces_stripped;
            }
        }
    }
}
