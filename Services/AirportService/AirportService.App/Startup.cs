using AutoMapper;
using Components;
using Components.CTeleport;
using Contracts.Components;
using FluentValidation;
using AirportService.App.Consumers.Airports;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Configuration;
using System;
using System.Net.Http;

namespace AirportService.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = configuration.Get<AirportServiceSettings>();
        }

        private readonly IConfiguration _configuration;
        private readonly AirportServiceSettings _appSettings;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(x => x.AddSerilog(dispose: true));

            // Add AutoMapper for all assemblies.
            services.AddAutoMapper(Program.Assemblies);

            // Add CQRS support for all assemblies.
            services.AddMediatR(Program.Assemblies);

            // Add validators for the FluentValidation.
            services.AddValidatorsFromAssemblies(Program.Assemblies);

            // Add the FluentValidation to the MediatR CQRS pipeline.
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatorValidationBehavior<,>));

            // Add the HTTP client factory support for the CTeleportDataProvider.
            services.AddHttpClient<IAirportInfoDataProvider, CTeleportDataProvider>();

            // Register dependency for the geographic distance calculator.
            services.AddSingleton<IGeoDistanceCalculator, StandardGeoDistanceCalculator>();

            // Register instances of the CTeleportDataProvider and its settings.
            services.AddSingleton(x => new CTeleportDataProviderSettings
            {
                AirportInfoRequestUrl = _appSettings.CTeleportDataProvider.AirportInfoRequestUrl
            });

            services.AddTransient<IAirportInfoDataProvider, CTeleportDataProvider>();

            // Add the MassTransit and RabbitMQ support.
            services.AddMessageBus(_appSettings, x => {

                // Register the necessary consumers.
                x.AddConsumer<GetAirportInfoConsumer>();
                x.AddConsumer<GetAirportDistanceConsumer>();
            });

            services
                .AddHealthChecksUI(setupSettings: setup =>
                {
                    if (Program.EnvironmentName != Environments.Production)
                    {
                        // SSL error prevention.

                        var clientHandler = new HttpClientHandler();

                        clientHandler
                            .ServerCertificateCustomValidationCallback =
                                (sender, cert, chain, sslPolicyErrors) => true;

                        setup.UseApiEndpointHttpMessageHandler(x => clientHandler);
                    }
                })
                .AddInMemoryStorage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hcapi", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.UIPath = "/hcui";
                    setup.ApiPath = "/health-ui-api";
                });
            });
        }
    }
}