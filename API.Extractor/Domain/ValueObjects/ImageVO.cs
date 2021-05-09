using API.Extractor.Domain.Interfaces;

namespace API.Extractor.Domain.VO
{
    public class ImageVO : IValueObject
    {
        public string Src { get; set; }
        public string Alt { get; set; }
        public ImageVO Clone()
        {
            return (ImageVO)this.MemberwiseClone();
        }
    }
}
