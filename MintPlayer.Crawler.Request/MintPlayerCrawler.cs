using MintPlayer.Crawler.Request.Data;
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

        public async Task<Subject> GetByUrl(string url)
        {
            try
            {
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
