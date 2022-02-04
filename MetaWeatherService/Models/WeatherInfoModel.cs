using System;
using System.Text.Json.Serialization;

namespace MetaWeatherService.Models
{
    public class WeatherInfoModel
    {
        private const double MILE_TO_KILOMETER = 1.60934;


        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("weather_state_name")]
        public string WeatherStateName { get; set; }

        [JsonPropertyName("weather_state_abbr")]
        public string WeatherStateAbbr { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("applicable_date")]
        public DateTime ApplicableDate { get; set; }

        /// <summary>
        /// Минимальная температура в градусах Цельсия
        /// </summary>
        [JsonPropertyName("min_temp")]
        public double? TemperatureMin { get; set; }

        /// <summary>
        ///  Максимальная температура в градусах Цельсия
        /// </summary>
        [JsonPropertyName("max_temp")]
        public double? TemperatureMax { get; set; }

        /// <summary>
        ///  Температура в градусах Цельсия
        /// </summary>
        [JsonPropertyName("the_temp")]
        public double? Temperature { get; set; }

        /// <summary>
        /// Скорость ветра в милях/час
        /// </summary>
        [JsonPropertyName("wind_speed")]
        public double WindSpeedMph { get; set; }

        /// <summary>
        /// Скорость ветра в км/ч
        /// </summary>
        [JsonIgnore]
        public double WindSpeedKph => WindSpeedMph * MILE_TO_KILOMETER;

        /// <summary>
        /// Скорость ветра в м/с
        /// </summary>
        [JsonIgnore]
        public double WindSpeedMps => WindSpeedMph * 0.44704;

        /// <summary>
        /// Направление ветра в градусах
        /// </summary>
        [JsonPropertyName("wind_direction")]
        public double WindDirection { get; set; }

        /// <summary>
        /// Направление ветра в точках компаса
        /// </summary>
        [JsonPropertyName("wind_direction_compass")]
        public string WindDirectionCompass { get; set; }

        /// <summary>
        /// Давление воздуха в mbar
        /// </summary>
        [JsonPropertyName("air_pressure")]
        public double? AirPressure { get; set; }

        /// <summary>
        /// Влажность в процентах
        /// </summary>
        [JsonPropertyName("humidity")]
        public int? Humidity { get; set; }

        /// <summary>
        /// Видимость в милях
        /// </summary>
        [JsonPropertyName("visibility")]
        public double? VisibilityMile { get; set; }

        /// <summary>
        /// Видимость в километрах
        /// </summary>
        [JsonIgnore]
        public double? VisibilityKm => VisibilityMile * MILE_TO_KILOMETER;

        /// <summary>
        /// Предсказуемость в процентах
        /// </summary>
        [JsonPropertyName("predictability")]
        public int Predictability { get; set; }
    }

}
