using API.Extractor.Interfaces;

namespace API.Extractor.VO
{
    public class WebsiteVO : IValueObject
    {
        public string Url { get; set; }
        public bool Download { get; set; }
    }
}
