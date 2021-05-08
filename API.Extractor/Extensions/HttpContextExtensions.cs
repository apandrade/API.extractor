using API.Extractor.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Extensions
{
    public static  class HttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseHttpContext(this IApplicationBuilder app)
        {
            ThisServer.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
    }
}
