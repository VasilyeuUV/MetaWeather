using MetaWeatherService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
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

            using IHost host = Hosting;
            await host.StartAsync();

            var weatherService = Services.GetRequiredService<MetaWeatherClient>();

            // - местоположение по названию
            var locationByName = await weatherService.GetLocation(relPaths.LocationByName, "Moscow");

            // - местоположение по координатам
            var locationByCoord = await weatherService.GetLocation(relPaths.LocationByCoord, locationByName[0].Coordinates);

            // - информация о погоде
            var weatherInfo = await weatherService.GetWeatheInfo(relPaths.InfoById, locationByName[0].Id);

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
                client.BaseAddress = new Uri(host.Configuration["MetaWeather"])     // - получаем базовый URL
                );
        }

        #endregion // Hosting

    }
}
