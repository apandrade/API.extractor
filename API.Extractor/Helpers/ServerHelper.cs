using API.Extractor.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Helpers
{
    public class ServerHelper
    {
        private static IHttpContextAccessor m_httpContextAccessor;

        public static HttpContext Current => m_httpContextAccessor.HttpContext;

        public static string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            m_httpContextAccessor = contextAccessor;
        }

        public static string MapWebRootPath(string path)
        {
            return Path.Combine(
                WebRootPath,
                path);
        }

        public static string MapContentRootPath(string path)
        {
            return Path.Combine(
                ContentRootPath,
                path);
        }

        public  static string ContentRootPath { get => (string)AppDomain.CurrentDomain.GetData("ContentRootPath"); }
        public static string WebRootPath { get => (string)AppDomain.CurrentDomain.GetData("WebRootPath"); }
    }
}
