using API.Extractor.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace API.Extractor.Models.Request
{
    public class ExtractorRequest : IModel, IRequestModel
    {
        [Required]
        public string Url { get; set; }

        public IValueObject ConvertToVo(Func<IValueObject> createVo)
        {
            return createVo();
        }

        public bool IsValid()
        {
            return ValidateUrl(Url);
        }

        public bool ValidateUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri validatedUri))
            {
                return (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
            }
            return false;
        }
    }
}
