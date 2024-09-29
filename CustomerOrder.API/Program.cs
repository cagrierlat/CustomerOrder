using CustomerOrder.API.Services.RabbitMq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerOrder.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.Console()
           .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // Logları buraya kaydet
           .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                // RabbitMQ tüketicisini başlatma
                var consumer = new RabbitMqConsumer();
                Task.Run(() => consumer.Start());

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
