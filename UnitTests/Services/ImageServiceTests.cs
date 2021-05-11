using API.Extractor.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace UnitTests.Services
{
    [TestClass]
    public class ImageServiceTests
    {
        private string _base64Image = "data:image/jpeg;base64,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";
        private string _imageUrl = "https://static.todamateria.com.br/upload/ma/ra/maracaturural-cke.jpg";
        private ImageService _imgService = new ImageService();
        private string _baseUrl = "https://localhost:44343";

        [TestInitialize]
        public void SetUp()
        {
            //var root = "D:\\dev\\APOA\\API.Extractor\\API.Extractor\\wwwroot";
            //D:\dev\APOA\API.Extractor\UnitTests\bin\Debug\netcoreapp3.1\
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            baseDirectory = baseDirectory.Replace("\\UnitTests\\bin\\Debug\\netcoreapp3.1\\", "");
            baseDirectory = $"{baseDirectory}\\API.Extractor\\wwwroot";
            AppDomain.CurrentDomain.SetData("WebRootPath", baseDirectory);
            Environment.SetEnvironmentVariable("IMAGES_PATH_NAME", "images");
        }
        [TestMethod]
        public void Should_Remove_Begining_Of_Base64String()
        {
            string sanitizedString = ImageService.SanitizeBase64String(_base64Image);
            Assert.IsFalse(sanitizedString.Contains("data:image/jpeg;"));
        }

        [TestMethod]
        public void Should_Return_Extension_Of_Base64String()
        {
            string extension = ImageService.GetExtensionFromBase64String(_base64Image);
            Assert.AreEqual(".jpeg", extension);
        }

        [TestMethod]
        public void Should_Save_Base64String()
        {
            string savedUrl = _imgService.DownloadAndSaveBase64Image(_base64Image, _baseUrl);
            Uri uriResult;
            bool result = Uri.TryCreate(savedUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            Assert.IsTrue(result);
            var fileName = Path.GetFileName(savedUrl);
            var filePath = Path.Combine(ImageService.ImageDirectory, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod]
        public void Should_Download_And_Save_Image_From_Url()
        {
            string savedUrl = _imgService.DownloadAndSaveImage(_imageUrl, _baseUrl);
            Uri uriResult;
            bool result = Uri.TryCreate(savedUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            Assert.IsTrue(result);
            var fileName = Path.GetFileName(savedUrl);
            var filePath = Path.Combine(ImageService.ImageDirectory, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod]
        public void Should_Return_File_Path()
        {
            string savedUrl = ImageService.GetFilePath(_imageUrl);
            Assert.IsTrue(Path.IsPathRooted(savedUrl));
        }

        [TestMethod]
        public void Should_Clear_Images_Directory()
        {
            ImageService.ClearImageDirectory();
            Assert.IsTrue(Directory.Exists(ImageService.ImageDirectory));
            Assert.IsFalse(Directory.EnumerateFileSystemEntries(ImageService.ImageDirectory).Any());
        }

        [TestMethod]
        public void Should_Return_FileExtension_From_URL()
        {
            string extension = ImageService.GetFileExtension(_imageUrl);
            Assert.AreEqual(".jpg", extension);
        }

        [TestMethod]
        public void Should__Return_A_Valid_URL()
        {


            string url = ImageService.GetImageUrlFromAbsolutePath(ImageService.GetFilePath(".png"), _baseUrl);
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            Assert.IsTrue(result);
        }




    }
}
