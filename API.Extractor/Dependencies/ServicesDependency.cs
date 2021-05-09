using API.Extractor.Dependencies.Extensions;
using API.Extractor.Domain.Interfaces;
using API.Extractor.Services;
using API.Extractor.Services.WebCrawlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using System;

namespace API.Extractor.Dependencies
{
    public static class ServicesDependency
    {

        public static IServiceCollection AddServiceDependency(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IControllerService, ExtractorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IWebCrawler, ChromeWebCrawler>();
            services.ConfigureProblemDetailsModelState();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Extractor API",
                        Version = "v1",
                        Description = "A very simple text and images extractor",
                        Contact = new OpenApiContact
                        {
                            Name = "André Andrade",
                            Url = new Uri("https://github.com/apandrade")
                        }
                    });
            });
            return services;
        }

        public static IApplicationBuilder Configure(this IApplicationBuilder app)
        {
            ContextService.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Extractor API V1");
            });

            app.UseStaticFiles();

            return app;
        }
    }

}
