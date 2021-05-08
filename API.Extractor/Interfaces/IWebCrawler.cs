using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Interfaces
{
    public interface IWebCrawler
    {
        public IWebDriver Driver { get; }
        public void SetUp();
        public IList<IValueObject> GetImageList(string sourceUrl);
        public IList<IValueObject> GetWordList(string sourceUrl);
    }
}
