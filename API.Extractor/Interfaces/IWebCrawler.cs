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
        public IList<IValueObject> GetList(string sourceUrl);
    }
}
