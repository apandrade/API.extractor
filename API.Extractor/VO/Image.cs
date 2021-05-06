using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;

namespace API.Extractor.VO
{
    public class Image : IValueObject
    {
        public Image(string src, string alt, int width, int height)
        {
            Src = src;
            Alt = alt;
            Width = width;
            Height = height;
        }
        public string Src { get; set; }
        public string Alt { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
