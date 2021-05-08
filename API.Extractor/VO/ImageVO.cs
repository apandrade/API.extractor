﻿using API.Extractor.Interfaces;

namespace API.Extractor.VO
{
    public class ImageVO : IValueObject
    {
        public string Src { get; set; }
        public string Alt { get; set; }
        public ImageVO Clone()
        {
            return (ImageVO)this.MemberwiseClone();
        }
    }
}
