using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Helpers
{
    public class UrlHelper
    {
        public static string SanitizeUrl(string baseUrl, string imageUrl)
        {
            baseUrl = baseUrl.TrimEnd('/');
            if (!IsAbsoluteUrl(imageUrl))
            {
                imageUrl = imageUrl.TrimStart('/');
            }
            return $"{baseUrl}/{imageUrl}";
        }
        public static bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        public static string GetUrlPath(string absoluteFilePath)
        {
            return absoluteFilePath.Replace(ServerHelper.WebRootPath, ServerHelper.AppBaseUrl).Replace('\\', '/');
        }
    }
}
