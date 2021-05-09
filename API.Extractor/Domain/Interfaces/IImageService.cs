using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Domain.Interfaces
{
    public interface IImageService : IService
    {
        public string DownloadAndSaveBase64Image(string base64String);
        public string DownloadAndSaveImage(string imageUrl);
    }
}
