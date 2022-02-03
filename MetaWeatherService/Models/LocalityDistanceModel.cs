using System.Text.Json.Serialization;

namespace MetaWeatherService.Models
{
    /// <summary>
    /// Модель данных о населенном пункте  (местности)
    /// </summary>
    public class LocalityDistanceModel : LocalityModel
    {

        /// <summary>
        /// Расстояние до точки местности в метрах
        /// </summary>
        [JsonPropertyName("distance")]
        public int Distance { get; set; }


        /// <summary>
        /// Перегрузка метода ToString(), чтобы видеть данные в отладчике
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Id}. {Type} {Title}({Coordinates.latitude}, {Coordinates.longitude}) - {Distance:0,0} m.";
    }
}
