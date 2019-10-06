using Newtonsoft.Json.Converters;

namespace MintPlayer.Crawler.Request.Converters
{
    internal class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
