using API.Extractor.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Services
{
    public static class ContextService
    {

        public static HttpContext CurrentContext => m_httpContextAccessor.HttpContext;
        private static IHttpContextAccessor m_httpContextAccessor;
        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            m_httpContextAccessor = contextAccessor;
        }

        public static string AppBaseUrl => $"{CurrentContext.Request.Scheme}://{CurrentContext.Request.Host}{CurrentContext.Request.PathBase}";
    }
}
