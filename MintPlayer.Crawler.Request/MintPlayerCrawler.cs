using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MintPlayer.Crawler.Request
{
    public class MintPlayerCrawler
    {
        private HttpClient httpClient;

        public MintPlayerCrawler()
        {
            httpClient = new HttpClient();
        }

        public async Task<Data.Subject> GetByUrl(string url)
        {
            try
            {
                var fetcher = Platforms.Fetcher.ByUrl(url);
                var subject = await fetcher.Fetch(httpClient);
                return subject;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
