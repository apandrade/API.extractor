using API.Extractor.Interfaces;
using API.Extractor.VO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace API.Extractor.WebCrawlers
{
    public class ChromeWebCrawler : IWebCrawler
    {
        public IWebDriver Driver { get; private set; }
        public ChromeWebCrawler()
        {
            SetUp();
        }
        public void SetUp()
        {
            if (Driver == null)
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--headless");
                var chromeWebdriverPath = Environment.GetEnvironmentVariable("CHROME_WEBDRIVER_PATH");
                Driver = new ChromeDriver(chromeWebdriverPath, chromeOptions);
            }
        }

        private void LoadPage(string url)
        {
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Driver.Navigate().GoToUrl(url);
        }

        public IList<IValueObject> GetList(string url)
        {
            IList<IValueObject> result = new List<IValueObject>();
            LoadPage(url);
            var images = Driver.FindElements(By.TagName("img"));
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
            Driver.Quit();
            Driver = null;
        }
    }
}
