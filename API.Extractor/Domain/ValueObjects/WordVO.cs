using API.Extractor.Domain.Interfaces;

namespace API.Extractor.Domain.VO
{
    public class WordVO : IValueObject
    {
        public string Word { get; set; }
        public int Count { get; set; }
    }
}
