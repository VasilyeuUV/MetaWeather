//using MetaWeatherService.Converters;
using MetaWeatherService.Models;
using System;
using System.Collections.Generic;
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
        private readonly Dictionary<int, string> _relationPaths = new()
        {
            [1] = "/api/location/search/?query=",       // поиск по наименованию:                 /api/location/search/?query=(query)
            [2] = "/api/location/search/?lattlong=",    // поиск по координатам:                  /api/location/search/?lattlong=(latt),(long)
            [3] = "/api/location/{0}/",                 // данные по Id места:                    /api/location/(woeid)/
            [4] = "/api/location/{0}/{1}/"              // данные по Id места и дате yyyy/mm/dd   /api/location/(woeid)/(date)/ (/api/location/44418/2013/4/27/)
        };

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
            string name,
            //IProgress<double> progress = null, 
            CancellationToken cancel = default)
        {
            return await _client
                .GetFromJsonAsync<LocationDistanceModel[]>($"{_relationPaths[1]}{name}", /*__jsonOptions,*/ cancel)    // __jsonOptions - настройки сериализации
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Получить данные местности по координатам
        /// </summary>
        public async Task<LocationDistanceModel[]> GetLocation(
            (double latitude, double longitude) location,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            var lat = location.latitude.ToString(CultureInfo.InvariantCulture);
            var lon = location.longitude.ToString(CultureInfo.InvariantCulture);
            return await _client
                .GetFromJsonAsync<LocationDistanceModel[]>($"{_relationPaths[2]}{lat},{lon}", cancel)
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
            int id,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            return await _client
                .GetFromJsonAsync<LocationWeatherInfoModel>(string.Format(_relationPaths[3], id), cancel)
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
            LocationModel locality,
            //IProgress<double> progress = null,
            CancellationToken cancel = default) => GetWeatheInfo(locality.Id, cancel);


        /// <summary>
        /// Получение информации о погоде по id местности и дате
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="dtg"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<WeatherInfoModel[]> GetWeatherInfo(
            int id,
            DateTime dtg,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            var stringDate = $"{dtg:yyyy}/{dtg:MM}/{dtg:dd}";
            return await _client
                .GetFromJsonAsync<WeatherInfoModel[]>(string.Format(_relationPaths[4], id, stringDate), cancel)
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
            LocationModel locality,
            DateTime dtg,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
            => GetWeatherInfo(locality.Id, dtg, cancel);

        public Task<WeatherInfoModel[]> GetWeatherInfo(
            LocationWeatherInfoModel locality,
            DateTime dtg,
            //IProgress<double> progress = null,
            CancellationToken cancel = default)
            => GetWeatherInfo(locality.Id, dtg, cancel);

        #endregion // Данные о погоде на местности
    }
}
