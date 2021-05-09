using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace API.Extractor.Helpers
{
    public class ImageHelper
    {
        private const string IMAGE_FILE_NAME_PREFIX = "image_";
        public static string ImageDirectory { get 
            {
                var imagePathName = Environment.GetEnvironmentVariable("IMAGES_PATH_NAME");
                return ServerHelper.MapWebRootPath($"{imagePathName}");
            } 
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

        public static string DownloadAndSaveBase64Image(string base64String)
        {
            string imageSavedUrl = "";
            string fileExtension = ImageHelper.GetExtensionFromBase64String(base64String);
            string filePath = FileSystemHelper.GetFilePath(ImageDirectory, IMAGE_FILE_NAME_PREFIX, fileExtension);
            base64String = ImageHelper.SanitizeBase64String(base64String);
            byte[] bytes = Convert.FromBase64String(base64String);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                image.Save(filePath);
                imageSavedUrl = UrlHelper.GetUrlPath(filePath);
            }

            return imageSavedUrl;
        }

        public static string DownloadAndSaveImage(string imageUrl)
        {
            string fileExtension = UrlHelper.GetFileExtension(imageUrl);
            string filePath = FileSystemHelper.GetFilePath(ImageDirectory, IMAGE_FILE_NAME_PREFIX, fileExtension);
            string savedImageUrl = UrlHelper.GetUrlPath(filePath);

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

    }
}
