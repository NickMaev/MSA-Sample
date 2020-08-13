using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Shared.MessageBus;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shared.Configuration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the MassTransit and RabbitMQ support.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSettingsBase">Application settings instance.</param>
        /// <param name="busConfigurator"></param>
        /// <param name="customRmqConfigurator"></param>
        public static void AddMessageBus(
            this IServiceCollection services,
            AppSettingsBase appSettingsBase,
            Action<IServiceCollectionBusConfigurator> busConfigurator,
            Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> customRmqConfigurator = null
            )
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (appSettingsBase == null)
            {
                throw new ArgumentNullException(nameof(appSettingsBase));
            }

            if (busConfigurator == null)
            {
                throw new ArgumentNullException(nameof(busConfigurator));
            }

            services.AddMassTransit(x =>
            {
                // Configure specific receive endpoints and consumers.
                busConfigurator(x);
                                
                x.UsingRabbitMq((context, rmqConfig) =>
                {
                    rmqConfig.Host(appSettingsBase.RabbitMq.Host, "/", hostConfig =>
                    {
                        hostConfig.Username(appSettingsBase.RabbitMq.Username);
                        hostConfig.Password(appSettingsBase.RabbitMq.Password);
                        hostConfig.RequestedConnectionTimeout(appSettingsBase.RabbitMq.TimeoutInMilliseconds);
                    });

                    // Create endpoints automatically for consumers.
                    rmqConfig.ConfigureEndpoints(context);

                    rmqConfig.AutoDelete = true;

                    rmqConfig.UseHealthCheck(context);

                    customRmqConfigurator?.Invoke(context, rmqConfig);
                });

            });

            services.AddMassTransitHostedService();

            services.AddTransient(typeof(IBusClient<,>), typeof(BusClient<,>));
        }

        public static void AddSwagger(this IServiceCollection services, AppSettingsBase appSettingsBase, Assembly[] assemblies)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (appSettingsBase == null)
            {
                throw new ArgumentNullException(nameof(appSettingsBase));
            }

            services.AddSwaggerGen(c => {

                c.SwaggerDoc(appSettingsBase.ApiVersion, new OpenApiInfo
                {
                    Version = appSettingsBase.ApiVersion,
                    Title = appSettingsBase.ProjectFullName,
                    Description = appSettingsBase.ProjectDescription,
                    Contact = new OpenApiContact
                    {
                        Name = appSettingsBase.AuthorName,
                        Email = appSettingsBase.AuthorEmail,
                        Url = new Uri(appSettingsBase.AuthorUrl),
                    },
                    License = new OpenApiLicense
                    {
                        Name = appSettingsBase.License
                    }
                });

                c.CustomSchemaIds(x => x.FullName);

                // Set the comments path for the Swagger JSON and UI.
                var existingXmlPaths =
                    assemblies
                    .SelectMany(x => x.GetReferencedAssemblies())
                    .Concat(assemblies.Select(x => x.GetName()))
                    .Distinct()
                    .Select(x => x.Name)
                    .Concat(new[] { $"{Assembly.GetExecutingAssembly().GetName().Name}" })
                    .Distinct()
                    .Select(x => $"{Path.Combine(AppContext.BaseDirectory, x)}.xml")
                    .Where(File.Exists)
                    .ToArray();

                foreach (var xmlPath in existingXmlPaths)
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }
    }
}