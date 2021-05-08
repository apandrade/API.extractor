using API.Extractor.Interfaces;
using API.Extractor.VO;
using System.Collections.Generic;

namespace API.Extractor.Models.Response
{
    public class ExtractorResponse : IModel, IResponseModel
    {
        public ExtractorResponse(IList<IValueObject> images)
        {
            Images = images;
        }
        public IList<IValueObject> Images { get; set; }
    }
}
