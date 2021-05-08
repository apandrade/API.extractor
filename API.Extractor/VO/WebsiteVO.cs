using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;

namespace API.Extractor.VO
{
    public class WebsiteVO : IValueObject
    {
        public string Url { get; set; }
        public bool Download { get; set; }
    }
}
