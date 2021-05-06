using API.Extractor.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;

namespace API.Extractor.Models.Response
{
    public class ExtractorResponse : IModel, IResponseModel
    {
        public ExtractorResponse(IList<IValueObject> images)
        {
            Images = images;
        }
        public IList<IValueObject>Images { get; set; }
    }
}
