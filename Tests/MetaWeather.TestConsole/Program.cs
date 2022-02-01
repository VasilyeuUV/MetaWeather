using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace MetaWeather.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host = Hosting;
            await host.StartAsync();

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
            // регистрируем сервисы

        }

        #endregion // Hosting

    }
}
