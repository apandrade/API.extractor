using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Domain.Interfaces
{
    public interface IWebCrawler : IService
    {
        public IWebDriver Driver { get; }
        public IList<IValueObject> GetImageList(string sourceUrl);
        public IList<IValueObject> GetWordList(string sourceUrl);
    }
}
