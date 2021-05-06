using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;

namespace API.Extractor.VO
{
    public class Website : IValueObject
    {
        public Website(string url)
        {
            Url = url;
        }
        string Url { get; set; }
    }
}
