using API.Extractor.Domain.Interfaces;

namespace API.Extractor.Domain.VO
{
    public class WebsiteVO : IValueObject
    {
        public string Url { get; set; }
        public bool Download { get; set; }
    }
}
