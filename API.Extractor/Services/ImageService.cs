using API.Extractor.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;

namespace API.Extractor.Services
{
    public class ImageService : BaseService, IImageService
    {
        public ImageService(IHttpContextAccessor contextAccessor): base(contextAccessor)
        {
            Configure();
        }
        private const string IMAGE_FILE_NAME_PREFIX = "image_";
        public string Name { get; private set; }
        public static string ImageDirectory
        {
            get
            {
                var imagePathName = Environment.GetEnvironmentVariable("IMAGES_PATH_NAME");
                return Path.Combine(WebRootPath, imagePathName);
            }
        }
        private static string WebRootPath { get => (string)AppDomain.CurrentDomain.GetData("WebRootPath"); }

        public void Configure()
        {
            Name = "ImageService";
        }
        public static string SanitizeBase64String(string base64)
        {
            var pieces = base64.Split(";base64,");
            return pieces.Count() == 2 ? pieces[1] : base64;
        }

        public static string GetExtensionFromBase64String(string base64)
        {
            var pieces = base64.Split(";");
            pieces = pieces[0].Split("/");
            return pieces.Count() == 2 ? $".{pieces[1]}" : "";
        }

        public string DownloadAndSaveBase64Image(string base64String, string appBaseUrl)
        {
            string imageSavedUrl = "";
            string fileExtension = GetExtensionFromBase64String(base64String);
            string filePath = GetFilePath(fileExtension);
            base64String = SanitizeBase64String(base64String);
            byte[] bytes = Convert.FromBase64String(base64String);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                image.Save(filePath);
                imageSavedUrl = GetImageUrlFromAbsolutePath(filePath, appBaseUrl);
            }

            return imageSavedUrl;
        }

        public string DownloadAndSaveImage(string imageUrl, string appBaseUrl)
        {
            string fileExtension = GetFileExtension(imageUrl);
            string filePath = GetFilePath(fileExtension);
            string savedImageUrl = GetImageUrlFromAbsolutePath(filePath, appBaseUrl);

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(imageUrl);
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Timeout = 30000;
            WebResponse webResponse = webRequest.GetResponse();

            using (Stream stream = webResponse.GetResponseStream())
            using (FileStream fs = File.Create(filePath))
            {
                stream.CopyTo(fs);
            }

            return savedImageUrl;
        }

        public static string GetFilePath(string extension)
        {
            var fullPath = "";
            do
            {
                fullPath = Path.Combine(ImageDirectory, $"{IMAGE_FILE_NAME_PREFIX}{Guid.NewGuid()}{extension}");
            } while (File.Exists(fullPath));


            return fullPath;
        }

        public static void ClearImageDirectory()
        {
            if (Directory.Exists(ImageDirectory))
            {
                Directory.Delete(ImageDirectory, true);
            }
            Directory.CreateDirectory(ImageDirectory);
        }

        public static string GetFileExtension(string url)
        {
            var urlPieces = url.Split('?');
            string result = Path.GetExtension(urlPieces[0]);
            return result ?? "";
        }

        public static string GetImageUrlFromAbsolutePath(string absoluteFilePath, string baseUrl)
        {
            return absoluteFilePath.Replace(WebRootPath, baseUrl).Replace('\\', '/');
        }
    }
}
