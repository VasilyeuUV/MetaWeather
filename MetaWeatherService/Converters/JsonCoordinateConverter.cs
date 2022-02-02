using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MetaWeatherService.Converters
{
    /// <summary>
    /// Конвертер широты и долготы
    /// </summary>
    internal class JsonCoordinateConverter : JsonConverter<(double latitude, double longitude)>
    {
        /// <summary>
        /// Для десереализации
        /// </summary>
        /// <param name="reader">объект, который создается на время чтения json-документа, является структурой</param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override (double latitude, double longitude) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            reader.GetString() is not { Length: >= 3 } str
            || str.Split(',') is not { Length: 2 } components
            || !double.TryParse(components[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var lat)
            || !double.TryParse(components[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var lon)
                ? (double.NaN, double.NaN)
                : (lat, lon);


        /// <summary>
        /// Для сериализации в Json
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, (double latitude, double longitude) value, JsonSerializerOptions options) => writer
            .WriteStringValue($"{value.latitude.ToString(CultureInfo.InvariantCulture)}, {value.longitude.ToString(CultureInfo.InvariantCulture)}");

    }
}
