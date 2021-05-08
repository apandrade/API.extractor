using API.Extractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;
using API.Extractor.Models.Response;
using API.Extractor.VO;
using System.Drawing;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using API.Extractor.Helpers;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using API.Extractor.Extensions;

namespace API.Extractor.Services
{
    public class ExtractorService : IService
    {
        public async Task<IResponseModel> Process(IValueObject vo, Func<object, IResponseModel> createResponse)
        {
            IList<ImageVO> images = await ExtractAllImages(((WebsiteVO)vo).Url);
            return await Task.Run(() => createResponse(images));
        }

        private async Task<IList<ImageVO>> ExtractAllImages(string url)
        {
            ClearImagesDirectory();
            IList<ImageVO> imagesList = GetAllImagesFullUrl(url);
            var downloadImagesTask = Task.Run(() => DownloadAndSaveImages(imagesList));
            return await downloadImagesTask;
        }

        private IList<ImageVO> GetAllImagesFullUrl(string url)
        {
            var document = new HtmlWeb().Load(url);
            var imgElements = document.DocumentNode.Descendants("img");

            return  imgElements.Select(e => new ImageVO
            {
                Src = SanitizeUrl(url, e.GetAttributeValue("src", "")),
                Alt = e.GetAttributeValue("alt", "")
            })
            .Where(i => !String.IsNullOrEmpty(i.Src))
            .ToList();
        }

        private async Task<IList<ImageVO>> DownloadAndSaveImages(IList<ImageVO> images)
        {

            var downloadImageTask = Task.Run(() =>
            {
                IList<ImageVO> result = new List<ImageVO>();
                foreach (ImageVO imageVO in images)
                {
                    if(Path.HasExtension(imageVO.Src))
                    {
                        ImageVO imageVOResult = imageVO.Clone();                    
                        imageVOResult.Src = DownloadAndSaveImageFromUrl(imageVO.Src);
                        result.Add(imageVOResult);
                    }                    
                }
                return result;
            });
            return await downloadImageTask;
        }

        private string SanitizeUrl(string baseUrl, string imageUrl)
        {
            baseUrl = baseUrl.TrimEnd('/');
            if (!IsAbsoluteUrl(imageUrl))
            {
                imageUrl = imageUrl.TrimStart('/');
            }
            return $"{baseUrl}/{imageUrl}";
        }
        private bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        public string DownloadAndSaveImageFromUrl(string imageUrl)
        {
            Image image = null;

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(imageUrl);
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Timeout = 30000;
            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            image = Image.FromStream(stream);
            webResponse.Close();

            string filePath = GetFilePath(Path.GetExtension(imageUrl));
            image.Save(filePath);
            string savedImageUrl = GetUrlPath(filePath);
            return savedImageUrl;
        }

        private string GetUrlPath(string absoluteFilePath)
        {
            return absoluteFilePath.Replace(ThisServer.WebRootPath, ThisServer.AppBaseUrl).Replace('\\', '/');
        }

        private string GetFilePath(string extension)
        {
            var directory = GetImagesDirectory();
            var fullPath = "";
            do
            {
                fullPath = Path.Combine(directory, $"image_{Guid.NewGuid()}{extension}");
            } while (File.Exists(fullPath));


            return fullPath;
        }
        private string GetImagesDirectory()
        {
            var imagePathName = Environment.GetEnvironmentVariable("IMAGES_PATH_NAME");
            return ThisServer.MapWebRootPath($"{imagePathName}");
        }
        private void ClearImagesDirectory()
        {
            var directory = GetImagesDirectory();
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);
        }
    }
}
