using API.Extractor.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;

namespace API.Extractor.Models.Request
{
    public class ExtractorRequest : IModel, IRequestModel
    {
        public string Url { get; set; }

        public IValueObject ConvertToVo(Func<IValueObject> createVo)
        {
            return createVo();
        }
    }
}
