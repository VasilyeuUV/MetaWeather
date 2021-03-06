using MetaWeatherService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MetaWeather.TestConsole
{
    class Program
    {

        static async Task Main(string[] args)
        {
            // Получаем данные из конфигурации
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            RelationPaths relPaths = config.GetRequiredSection("RelationPaths").Get<RelationPaths>();
            // использование: relPaths.LocationByName, relPaths.LocationByCoord, ...
            // переделка: данные из конфигурации далее не используются. относительные пути вынесены в Клиент.

            using IHost host = Hosting;
            await host.StartAsync();

            var weatherService = Services.GetRequiredService<MetaWeatherClient>();

            // - местоположение по названию
            var locationByName = await weatherService.GetLocation("Moscow");

            // - местоположение по координатам
            var locationByCoord = await weatherService.GetLocation(locationByName[0].Coordinates);

            // - информация о погоде по id местности
            var weatherInfoByLocalityId = await weatherService.GetWeatheInfo(locationByName[0].Id);

            // - информация о погоде по объекту меcтности
            var weatherInfoByLocality = await weatherService.GetWeatheInfo(locationByName[0]);

            // - информация о погоде по id местности и дате
            var weatherInfoByDateLocalityId = await weatherService.GetWeatherInfo(locationByName[0].Id, DateTime.Now);

            // - информация о погоде по объекту местности и дате
            var weatherInfoByDateLocality = await weatherService.GetWeatherInfo(locationByName[0], DateTime.Now);

            Console.WriteLine("Завершено");
            Console.ReadLine();

            await host.StopAsync();
        }


        //########################################################################################
        #region Services

        /// <summary>
        /// Сервисы
        /// </summary>
        public static IServiceProvider Services => Hosting.Services;

        #endregion // Services


        //########################################################################################
        #region Host

        private static IHost __hosting;
        /// <summary>
        /// Host
        /// </summary>
        public static IHost Hosting => __hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        /// <summary>
        /// Построитель хоста
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)                     
            .ConfigureServices(ConfigureServices);          // конфигурируем сервисы (контейнер сервисов)

        /// <summary>
        /// Конфигуратор сервисов
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            // РЕГИСТРАЦИЯ СЕРВИСОВ

            // - сервис получения данных о погоде
            services.AddHttpClient<MetaWeatherClient>(client =>                     // - конфигурация сервиса
                client.BaseAddress = new Uri(host.Configuration["MetaWeather"]))    // - получаем базовый URL
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))                        // - время жизни клиента (нужны установленные расширения Ms.Ext.Http.Polly и Polly.Extentions.Http)
                .AddPolicyHandler(GetRetryPolicy())                                 // - дополнительный политика клиента
                ;
        }


        /// <summary>
        /// Политики клиента
        /// </summary>
        /// <returns></returns>
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // Джиттер - для обеспечение возможности рассинхронизации доступа
            // при большом количестве запросов на сервер от разных клиентов
            var jitter = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()                                         // - перехватываем возможные ошибки процесса соединения с сервером
                .WaitAndRetryAsync(                                                 // - если сервер недоступен, то Http повторит запрос
                    6,                                                              // -- 6 раз
                    retry_attempt =>                                                // -- номер повтора
                        TimeSpan.FromSeconds(Math.Pow(2, retry_attempt)) +          // --- через время с прогрессией в 2 секунды в зависимости от номера повтора
                        TimeSpan.FromMilliseconds(jitter.Next(0, 1000)))            // --- со случайной рассинхронизацией времени запроса в 1 миллисекунду
                ;
        }

        #endregion // Hosting

    }
}
