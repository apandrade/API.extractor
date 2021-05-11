using API.Extractor.Domain.Interfaces;
using API.Extractor.Domain.VO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Extractor.Services
{
    public class ExtractorService : BaseService,  IControllerService
    {
        IList<string> _supportedImageTypes;
        private IWebCrawler _webCrawler;
        private IImageService _imageService;

        public string Name { get; private set; }
        public ExtractorService(IServiceProvider serviceProvider, IHttpContextAccessor contextAccessor):base(contextAccessor)
        {
            _webCrawler = serviceProvider.GetRequiredService<IWebCrawler>();
            _imageService = serviceProvider.GetRequiredService<IImageService>();

            Configure();
        }
        public void Configure()
        {
            Name = "ExtractorService";
            _supportedImageTypes = new List<string> { ".png", ".jpg", ".jpeg", ".jfif", ".exif", ".bmp", ".tiff", ".tif", ".gif" };
        }
        public async Task<IResponseModel> Process(IValueObject vo, Func<object, object, IResponseModel> createResponse)
        {
            var websiteVO = (WebsiteVO)vo;
            IList<IValueObject> images = await ExtractAllImages(websiteVO.Url, websiteVO.Download);
            IList<IValueObject> words = await ExtractMostUsedWords(websiteVO.Url);
            return await Task.Run(() => createResponse(images, words));
        }

        private async Task<IList<IValueObject>> ExtractMostUsedWords(string url)
        {
            var wordListTask = Task.Run(() => _webCrawler.GetWordList(url));
            return await wordListTask;
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
            var result = _webCrawler.GetImageList(url);
            return result;
        }


        private async Task<IList<IValueObject>> DownloadAndSaveImages(IList<IValueObject> images)
        {
            ImageService.ClearImageDirectory();
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
                string extension = ImageService.GetFileExtension(imageUrl);
                if (IsSupportedFormat(extension))
                {
                    imageSavedUrl = _imageService.DownloadAndSaveImage(imageUrl, AppBaseUrl);
                }
            }
            else
            {
                imageSavedUrl = _imageService.DownloadAndSaveBase64Image(imageUrl, AppBaseUrl);
            }

            return imageSavedUrl;
        }
    }
}
