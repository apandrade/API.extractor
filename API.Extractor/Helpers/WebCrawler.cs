using API.Extractor.VO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace API.Extractor.Helpers
{
    public class WebCrawler
    {
        private IWebDriver _driver;
        public WebCrawler()
        {
            SetUp();
        }
        public void SetUp()
        {
            if (_driver == null)
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--headless");
                var chromeWebdriverPath = Environment.GetEnvironmentVariable("CHROME_WEBDRIVER_PATH");
                _driver = new ChromeDriver(chromeWebdriverPath, chromeOptions);
            }
        }

        private void LoadPage(string url)
        {
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Navigate().GoToUrl(url);
        }

        public IList<ImageVO> GetAllImagesFromUrl(string url)
        {
            IList<ImageVO> result = new List<ImageVO>();
            LoadPage(url);
            var images = _driver.FindElements(By.TagName("img"));
            foreach (var image in images)
            {
                var src = image.GetAttribute("src");
                var alt = image.GetAttribute("alt");
                if (!string.IsNullOrEmpty(src))
                {
                    result.Add(new ImageVO { Src = src, Alt = alt });
                }
            }
            return result;
        }

        public void Close()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
