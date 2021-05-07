using API.Extractor.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;
using System.ComponentModel.DataAnnotations;
using API.Extractor.Validators;

namespace API.Extractor.Models.Request
{
    public class ExtractorRequest : IModel, IRequestModel
    {
        [UrlValidator]
        [Required]
        public string Url { get; set; }

        public IValueObject ConvertToVo(Func<IValueObject> createVo)
        {
            return createVo();
        }
    }
}
