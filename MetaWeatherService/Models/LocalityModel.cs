using MetaWeatherService.Converters;
using MetaWeatherService.Enums;
using System.Text.Json.Serialization;

namespace MetaWeatherService.Models
{
    /// <summary>
    /// Модель данных о населенном пункте  (местности)
    /// </summary>
    public class LocalityModel
    {
        /// <summary>
        /// Id местности
        /// </summary>
        [JsonPropertyName("woeid")]
        public int Id { get; set; }
       
        /// <summary>
        /// Наименование местности
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        /// <summary>
        /// Тип местности в виде перечисления c использованием дефолтного конвертера
        /// </summary>
        [JsonPropertyName("location_type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LocationTypeEnum Type { get; set; }        
        
        /// <summary>
        /// Координаты точки местности с использованием пользовательского конвертера
        /// </summary>
        [JsonPropertyName("latt_long")]
        [JsonConverter(typeof(JsonCoordinateConverter))]
        public (double latitude, double longitude) Location { get; set; }       // - используем кортеж для координат

        /// <summary>
        /// Расстояние до точки местности в метрах
        /// </summary>
        [JsonPropertyName("distance")]
        public int Distance { get; set; }

    }
}
