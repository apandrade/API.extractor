using API.Extractor.Domain.Interfaces;
using API.Extractor.Domain.VO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Services.WebCrawlers
{
    public class ChromeWebCrawler : BaseService,  IWebCrawler
    {
        readonly IConfiguration _configuration;
        public IWebDriver Driver { get; private set; }
        private string _symbolsAndNumbers = "/?:;][}{~^´`.,ª=§+-_)(*&¨%¢¬$£#³@¹!\'\"²1234567890\\|";

        public string Name { get; private set; }
        public int MinWordSize { get; private set; }
        public ChromeWebCrawler(IConfiguration configuration, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _configuration = configuration;
            Configure();
        }
        public void Configure()
        {
            Name = "ChromeWebCrawler";
            Int32.TryParse(_configuration["MinWordSize"], out int minWordSize);
            MinWordSize = minWordSize <= 0 ? 1 : minWordSize;
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
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(300);
            Driver.Navigate().GoToUrl(url);
        }

        public IList<IValueObject> GetWordList(string url)
        {
            IList<IValueObject> result = new List<IValueObject>();
            LoadPage(url);
            WaitAndScrollToBottom();
            var wholeText = Driver.FindElement(By.TagName("body")).Text.Trim();
            //Dictionary<string, int> wordCount = new Dictionary<string, int>();
            var wordList = wholeText.Split(" ");


            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };
            ConcurrentDictionary<string, int> wordCount = new ConcurrentDictionary<string, int>();
            int currentCount;

            Parallel.ForEach(wordList, parallelOptions, (word) =>
            {
                var key = word.Trim();
                if (key.Trim().Length < MinWordSize && 
                (!String.IsNullOrEmpty(key.Trim()) || 
                !String.IsNullOrWhiteSpace(key.Trim()) || 
                !_symbolsAndNumbers.Contains(key)))
                {
                    wordCount.TryGetValue(key, out currentCount);
                    wordCount[key] = ++currentCount;
                }
  
            });

            var top10 = wordCount.OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key).Take(10);
            result = top10.Select(x => new WordVO { Word = x.Key, Count = x.Value }).ToList<IValueObject>();           
            return result;
        }

        private void WaitAndScrollToBottom()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }


        public IList<IValueObject> GetImageList(string url)
        {
            IList<IValueObject> result = new List<IValueObject>();
            LoadPage(url);
            WaitAndScrollToBottom();
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };
            var images = Driver.FindElements(By.TagName("img"));

            Parallel.ForEach(images, parallelOptions, (image) =>
            {
                var src = image.GetAttribute("src");
                var alt = image.GetAttribute("alt");
                if (!string.IsNullOrEmpty(src))
                {
                    result.Add(new ImageVO { Src = src, Alt = alt });
                }
            });

            return result;
        }

        public void Close()
        {
            Driver.Quit();
            Driver = null;
        }
    }
}
