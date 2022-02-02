using MetaWeatherService.Converters;
using MetaWeatherService.Models;
using System.Net.Http;
using System.Net.Http.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace MetaWeatherService
{
    public class MetaWeatherClient
    {
        private readonly HttpClient _client;

        // ВАРИАНТ ИСПОЛЬЗОВАНИЯ КОНВЕРТЕРОВ
        ///// <summary>
        ///// Настройки JSON сериализации
        ///// </summary>
        //private static readonly JsonSerializerOptions __jsonOptions = new()
        //{
        //    // - конвертеры
        //    Converters =
        //    {
        //        new JsonStringEnumConverter(),      // - ковертер строки в перечисление
        //        new JsonCoordinateConverter(),      // - конвертер строки в координаты 
        //    }
        //};


        /// <summary>
        /// CTOR
        /// </summary>
        public MetaWeatherClient(HttpClient client) => this._client = client;


        /// <summary>
        /// Получить данные населенного пункта по его имени
        /// </summary>
        /// <param name="name">Название города</param>
        /// <returns></returns>
        public async Task<LocalityModel[]> GetLocation(string query, CancellationToken cancel = default)
        {            
            return await _client
                .GetFromJsonAsync<LocalityModel[]>(query, /*__jsonOptions,*/ cancel)    // __jsonOptions - настройки сериализации
                .ConfigureAwait(false);
        }

    }
}
