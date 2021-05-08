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
using API.Extractor.Helpers;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using API.Extractor.Extensions;

namespace API.Extractor.Services
{
    public class ExtractorService : IService
    {
        IList<string> _supportedImageTypes = new List<string> { ".png", ".jpg", ".jpeg", ".jfif", ".bmp", ".tiff", ".tif", ".gif"};
        public async Task<IResponseModel> Process(IValueObject vo, Func<object, IResponseModel> createResponse)
        {
            var websiteVO = (WebsiteVO)vo;
            IList<ImageVO> images = await ExtractAllImages(websiteVO.Url, websiteVO.Download);
            return await Task.Run(() => createResponse(images));
        }

        private async Task<IList<ImageVO>> ExtractAllImages(string url, bool mustDownload)
        {
            var imageListTask = Task.Run(() => GetAllImagesFullUrl(url)); 

            if(mustDownload)
            {
                IList<ImageVO> imageList = await imageListTask;
                var downloadImagesTask = Task.Run(() => DownloadAndSaveImages(imageList));
                return await downloadImagesTask;
            }

            return await imageListTask;
        }

        private IList<ImageVO> GetAllImagesFullUrl(string url)
        {
            var crawler = new WebCrawler();
            var result = crawler.GetAllImagesFromUrl(url);
            crawler.Close();
            return result;
        }


        private async Task<IList<ImageVO>> DownloadAndSaveImages(IList<ImageVO> images)
        {
            ClearImagesDirectory();
            var downloadImageTask = Task.Run(() =>
            {
                IList<ImageVO> result = new List<ImageVO>();
                foreach (ImageVO imageVO in images)
                {

                    string extension = GetFileExtension(imageVO.Src);
                    if(IsSupportedFormat(extension))
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
        private bool IsSupportedFormat(string extension)
        {
            bool result = false;
            if(!String.IsNullOrEmpty(extension) && _supportedImageTypes.Contains(extension))
            {
                return true;
            }
            return result;
        }
        private string GetFileExtension(string url)
        {
            var urlPieces = url.Split('?');
            string result = Path.GetExtension(urlPieces[0]);
            return result ?? "";
        }
        public string DownloadAndSaveImageFromUrl(string imageUrl)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(imageUrl);
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Timeout = 30000;
            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            Image image = Image.FromStream(stream);
            webResponse.Close();

            string filePath = GetFilePath(GetFileExtension(imageUrl));
            image.Save(filePath);
            string savedImageUrl = UrlHelper.GetUrlPath(filePath);
            return savedImageUrl;
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
            return ServerHelper.MapWebRootPath($"{imagePathName}");
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
