using Microsoft.AspNetCore.Http;

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
