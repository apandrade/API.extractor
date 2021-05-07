using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Validators
{
    public class UrlValidator : ValidationAttribute
    {
        public UrlValidator() : base("The URL is invalid") { }

        public override bool IsValid(object value)
        {
            return Validate((string)value);
        }
        private bool Validate(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri validatedUri))
            {
                return (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
            }
            return false;
        }
    }
}
