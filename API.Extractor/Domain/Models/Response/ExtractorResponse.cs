using API.Extractor.Domain.Interfaces;
using System.Collections.Generic;

namespace API.Extractor.Domain.Models.Response
{
    public class ExtractorResponse : IModel, IResponseModel
    {
        public ExtractorResponse(IList<IValueObject> images, IList<IValueObject> words)
        {
            Images = images;
            Words = words;
        }
        public IList<IValueObject> Images { get; set; }
        public IList<IValueObject> Words { get; set; }
    }
}
