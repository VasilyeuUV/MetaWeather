using MetaWeatherService.Converters;
using MetaWeatherService.Models;
using System;
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
        public async Task<LocationDistanceModel[]> GetLocation(
            string path, 
            string name, 
            //IProgress<double> progress = null, 
            CancellationToken cancel = default)
        {
            return await _client
                .GetFromJsonAsync<LocationDistanceModel[]>($"{path}{name}", /*__jsonOptions,*/ cancel)    // __jsonOptions - настройки сериализации
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Получить данные местности по координатам
        /// </summary>
        public async Task<LocationDistanceModel[]> GetLocation(
            string path, 
            (double latitude, double longitude) location,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            var lat = location.latitude.ToString(CultureInfo.InvariantCulture);
            var lon = location.longitude.ToString(CultureInfo.InvariantCulture);
            return await _client
                .GetFromJsonAsync<LocationDistanceModel[]>($"{path}{lat},{lon}", cancel)
                .ConfigureAwait(false);
        }

        #endregion // Данные о местности


        //##############################################################################################################
        #region Данные о погоде на местности

        /// <summary>
        /// Получение информации о погоде по id местности
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<LocationWeatherInfoModel> GetWeatheInfo(
            string path, 
            int id,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            return await _client
                .GetFromJsonAsync<LocationWeatherInfoModel>(string.Format(path, id), cancel)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Получение информации о погоде из объекта местности
        /// </summary>
        /// <param name="path"></param>
        /// <param name="weather"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public Task<LocationWeatherInfoModel> GetWeatheInfo(
            string path, 
            LocationModel locality,
            //IProgress<double> progress = null,
            CancellationToken cancel = default) => GetWeatheInfo(path, locality.Id, cancel);


        /// <summary>
        /// Получение информации о погоде по id местности и дате
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="dtg"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<WeatherInfoModel[]> GetWeatherInfo(
            string path,
            int id,
            DateTime dtg,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            var stringDate = $"{dtg:yyyy}/{dtg:MM}/{dtg:dd}";
            return await _client
                .GetFromJsonAsync<WeatherInfoModel[]>(string.Format(path, id, stringDate), cancel)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Получение информации о погоде по объекту местности и дате
        /// </summary>
        /// <param name="path"></param>
        /// <param name="locality"></param>
        /// <param name="dtg"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public Task<WeatherInfoModel[]> GetWeatherInfo(
            string path,
            LocationModel locality,
            DateTime dtg,
            //IProgress<double> progress = null,
            CancellationToken cancel = default) 
            => GetWeatherInfo(path, locality.Id, dtg, cancel);

        public Task<WeatherInfoModel[]> GetWeatherInfo(
            string path,
            LocationWeatherInfoModel locality,
            DateTime dtg,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
            => GetWeatherInfo(path, locality.Id, dtg, cancel);

        #endregion // Данные о погоде на местности
    }
}
