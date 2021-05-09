using API.Extractor.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Domain.VO
{
    public class WordVO : IValueObject
    {
        public string Word { get; set; }
        public int Count { get; set; }
    }
}
