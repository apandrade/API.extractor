namespace API.Extractor.Domain.Interfaces
{
    public interface IImageService : IService
    {
        public string DownloadAndSaveBase64Image(string base64String, string appBaseUrl);
        public string DownloadAndSaveImage(string imageUrl, string appBaseUrl);
    }
}
