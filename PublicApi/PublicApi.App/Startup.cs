using AutoMapper;
using Contracts.MessageBus.AirportService;
using FluentValidation;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PublicApi.App.Middlewares;
using Serilog;
using Shared.Configuration;
using System.Net.Http;

namespace PublicApi.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = configuration.Get<PublicApiSettings>();
        }

        private readonly IConfiguration _configuration;
        private readonly PublicApiSettings _appSettings;

        // This method gets called by the runtime. Use this method to add services to the container.
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

            // Add the MassTransit and RabbitMQ support.
            services.AddMessageBus(_appSettings, x => {
                x.AddRequestClient<GetAirportInfoRequest>();
                x.AddRequestClient<GetAirportDistanceRequest>();
            });

            services.AddControllers();

            services.AddSwagger(_appSettings, Program.Assemblies);

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c => { 
                c.SerializeAsV2 = true; 
            });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.).
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_appSettings.ProjectFullName} {_appSettings.ApiVersion}");
            });
        }
    }
}