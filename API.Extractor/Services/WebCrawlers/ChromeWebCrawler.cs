using API.Extractor.Domain.Interfaces;
using API.Extractor.Domain.VO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Extractor.Services.WebCrawlers
{
    public class ChromeWebCrawler : IWebCrawler
    {
        public IWebDriver Driver { get; private set; }
        private string _symbolsAndNumbers = "/?:;][}{~^´`.,ª=§+-_)(*&¨%¢¬$£#³@¹!\'\"²1234567890\\|";

        public string Name { get; private set; }
        public ChromeWebCrawler()
        {
            Configure();
        }
        public void Configure()
        {
            Name = "ChromeWebCrawler";
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

        public IList<IValueObject> GetWordList(string url)
        {
            IList<IValueObject> result = new List<IValueObject>();
            LoadPage(url);

            var wholeText = Driver.FindElement(By.TagName("body")).Text.Trim();
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            var wordList = wholeText.Split(" ");

            int currentCount;
            foreach (var word in wordList)
            {
                var key = word.Trim();
                if (String.IsNullOrEmpty(key.Trim()) || String.IsNullOrWhiteSpace(key.Trim()) || _symbolsAndNumbers.Contains(key))
                    continue;

                wordCount.TryGetValue(key,out currentCount);
                wordCount[key] = ++currentCount;
            }

            var top10 = wordCount.OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key).Take(10);
            foreach (KeyValuePair<string, int> entry in top10)
            {
                WordVO word = new WordVO { Word = entry.Key, Count = entry.Value };
                result.Add(word);
            }

            return result;
        }

        public IList<IValueObject> GetImageList(string url)
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
