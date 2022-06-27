using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Social.Network.SeedWorks.Models;
using System;
using System.IO;

namespace Social.Network
{
    public class Program
    {
        public static AppSettingsModel AppSettingsInfo { get; private set; }

        public static void Main(string[] args)
        {
            try
            {
                ReadAppSettingsInfo();

                var host = CreateWebHostBuilder(args).Build().Seed();
                host.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            builder.UseContentRoot(Directory.GetCurrentDirectory());

            builder = builder.UseHttpSys().UseUrls(AppSettingsInfo.HostAddress);

            return builder
                .UseStartup<Startup>();
        }

        public static void ReadAppSettingsInfo()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            AppSettingsInfo = config.Get<AppSettingsModel>();
        }
    }
}
