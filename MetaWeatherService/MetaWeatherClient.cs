using MetaWeatherService.Converters;
using MetaWeatherService.Models;
using System.Globalization;
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

        //##############################################################################################################
        #region Данные о местности

        /// <summary>
        /// Получить данные местности по названию
        /// </summary>
        /// <param name="name">Название города</param>
        /// <returns></returns>
        public async Task<LocalityDistanceModel[]> GetLocation(string path, string name, CancellationToken cancel = default)
        {
            return await _client
                .GetFromJsonAsync<LocalityDistanceModel[]>($"{path}{name}", /*__jsonOptions,*/ cancel)    // __jsonOptions - настройки сериализации
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Получить данные местности по координатам
        /// </summary>
        /// <param name="path">относительный путь</param>
        /// <param name="location">кортеж координат</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<LocalityDistanceModel[]> GetLocation(string path, (double latitude, double longitude) location, CancellationToken cancel = default)
        {
            var coordinates = $"{location.latitude.ToString(CultureInfo.InvariantCulture)},{location.longitude.ToString(CultureInfo.InvariantCulture)}";
            return await _client
                .GetFromJsonAsync<LocalityDistanceModel[]>($"{path}{coordinates}", cancel)
                .ConfigureAwait(false);
        }

        #endregion // Данные о местности


        //##############################################################################################################
        #region Данные о погоде

        /// <summary>
        /// Получение информации о погоде по id местности
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<WeatherInfoModel> GetWeatheInfo(string path, int id, CancellationToken cancel = default)
        {
            return await _client
                .GetFromJsonAsync<WeatherInfoModel>(string.Format(path, id, cancel))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Получение информации о погоде из объекта местности
        /// </summary>
        /// <param name="path"></param>
        /// <param name="weather"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public Task<WeatherInfoModel> GetWeatheInfo(string path, LocalityModel locality, CancellationToken cancel = default) =>
            GetWeatheInfo(path, locality.Id, cancel);

        #endregion // Данные о погоде
    }
}
