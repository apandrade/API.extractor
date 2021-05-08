using API.Extractor.Interfaces;
using API.Extractor.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Extractor.Models.Request
{
    public class ExtractorRequest : IModel, IRequestModel
    {
        [UrlValidator]
        [Required]
        public string Url { get; set; }
        [Required]
        public bool Download { get; set; }


        public IValueObject ConvertToVo(Func<IValueObject> createVo)
        {
            return createVo();
        }
    }
}
