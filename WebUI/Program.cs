using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Threading.Tasks;
using TicTacToe.Infrastructure.Persistence;

namespace WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            NLog.Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                IHost host = CreateHostBuilder(args).Build();
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    TicTacToeDbContext context = scope.ServiceProvider.GetRequiredService<TicTacToeDbContext>();
                    context.Database.Migrate();
                }

                await host.RunAsync();
            }
            catch (Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
        }
    }
}
