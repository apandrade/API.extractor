using OpenQA.Selenium;
using System.Collections.Generic;

namespace API.Extractor.Domain.Interfaces
{
    public interface IWebCrawler : IService
    {
        public IWebDriver Driver { get; }
        public IList<IValueObject> GetImageList(string sourceUrl);
        public IList<IValueObject> GetWordList(string sourceUrl);
    }
}
