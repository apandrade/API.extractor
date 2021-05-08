﻿using System;
using System.IO;

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

        public static string GetFileExtension(string url)
        {
            var urlPieces = url.Split('?');
            string result = Path.GetExtension(urlPieces[0]);
            return result ?? "";
        }
    }
}