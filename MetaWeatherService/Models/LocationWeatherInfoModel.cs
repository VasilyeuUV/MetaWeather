using System;
using System.Text.Json.Serialization;

namespace MetaWeatherService.Models
{
    /// <summary>
    /// Модель информации о погоде
    /// </summary>
    public class LocationWeatherInfoModel : LocationModel
    {

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("sun_rise")]
        public DateTime SunRise { get; set; }

        [JsonPropertyName("sun_set")]
        public DateTime SunSet { get; set; }

        [JsonPropertyName("timezone_name")]
        public string TimezoneName { get; set; }

        [JsonPropertyName("parent")]
        public LocationModel Locality { get; set; }

        [JsonPropertyName("timezone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("consolidated_weather")]
        public WeatherInfoModel[] LocalityWeatherInfo { get; set; }

        [JsonPropertyName("sources")]
        public Source[] Sources { get; set; }
        public class Source
        {
            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("slug")]
            public string Slug { get; set; }

            [JsonPropertyName("url")]
            public string Url { get; set; }

            [JsonPropertyName("crawl_rate")]
            public int CrawlRate { get; set; }
        }
    }
}
