using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PublicApi.App
{
    public class Program
    {
        public static string EnvironmentName =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production;

        public static Action<IConfigurationBuilder> BuildConfiguration =
            builder => builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

        /// <summary>
        /// Not for production use.
        /// </summary>
#warning Not for production use.
        public static Assembly[] Assemblies => 
                Assembly
                .GetExecutingAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .Concat(new[] { typeof(Startup).Assembly })
                .ToArray();

        public static int Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfiguration(builder);

            Log.Logger =
                new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .CreateLogger();

            try
            {
                var hostBuilder = CreateHostBuilder(args, builder);

                var host = hostBuilder.Build();

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationBuilder configurationBuilder)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseIISIntegration()
                    .ConfigureKestrel(serverOptions =>
                    {
                        // Set properties and call methods on options.
                    })
                    .UseConfiguration(
                        configurationBuilder
                        .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"hosting.{EnvironmentName}.json", optional: true)
                        .Build()
                    )
                    .UseStartup<Startup>();
                });
        }
    }
}