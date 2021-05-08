using API.Extractor.Helpers;
using API.Extractor.Interfaces;
using API.Extractor.VO;
using API.Extractor.WebCrawlers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace API.Extractor.Services
{
    public class ExtractorService : IService
    {
        IList<string> _supportedImageTypes = new List<string> { ".png", ".jpg", ".jpeg", ".jfif", ".exif", ".bmp", ".tiff", ".tif", ".gif" };
        public IWebCrawler WebCrawler { get; set; }
        public ExtractorService()
        {
            WebCrawler = new ChromeWebCrawler();
            WebCrawler.SetUp();
        }
        public async Task<IModel> Process(IValueObject vo, Func<object, IModel> createResponse)
        {
            var websiteVO = (WebsiteVO)vo;
            IList<IValueObject> images = await ExtractAllImages(websiteVO.Url, websiteVO.Download);
            return await Task.Run(() => createResponse(images));
        }

        private async Task<IList<IValueObject>> ExtractAllImages(string url, bool mustDownload)
        {
            var imageListTask = Task.Run(() => GetAllImagesFullUrl(url));

            if (mustDownload)
            {
                IList<IValueObject> imageList = await imageListTask;
                var downloadImagesTask = Task.Run(() => DownloadAndSaveImages(imageList));
                return await downloadImagesTask;
            }

            return await imageListTask;
        }

        private IList<IValueObject> GetAllImagesFullUrl(string url)
        {
            var result = WebCrawler.GetList(url);
            return result;
        }


        private async Task<IList<IValueObject>> DownloadAndSaveImages(IList<IValueObject> images)
        {
            FileSystemHelper.ClearDirectory(ImageHelper.ImageDirectory);
            var downloadImageTask = Task.Run(() =>
            {
                IList<IValueObject> result = new List<IValueObject>();
                foreach (ImageVO imageVO in images)
                {
                    ImageVO imageVOResult = imageVO.Clone();
                    imageVOResult.Src = DownloadAndSaveImageFromUrl(imageVO.Src);
                    if (!String.IsNullOrEmpty(imageVOResult.Src))
                    {
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
            if (!String.IsNullOrEmpty(extension) && _supportedImageTypes.Contains(extension))
            {
                return true;
            }
            return result;
        }
        public string DownloadAndSaveImageFromUrl(string imageUrl)
        {
            string imageSavedUrl = "";
            if (!imageUrl.StartsWith("data:image"))
            {
                string extension = UrlHelper.GetFileExtension(imageUrl);
                if (IsSupportedFormat(extension))
                {
                    imageSavedUrl = ImageHelper.DownloadAndSaveImage(imageUrl);
                }
            }
            else
            {
                imageSavedUrl = ImageHelper.DownloadAndSaveBase64Image(imageUrl);
            }

            return imageSavedUrl;
        }
    }
}
