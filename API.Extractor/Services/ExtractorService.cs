using API.Extractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extractor.Interfaces;
using API.Extractor.Models.Response;
using API.Extractor.VO;

namespace API.Extractor.Services
{
    public class ExtractorService : IService
    {
        public async Task<IResponseModel> Process(IValueObject vo)
        {
            IValueObject image = new Image("fake image", "fake image", 100, 100);
            IList<IValueObject> images = new List<IValueObject>();
            images.Add(image);
            return new ExtractorResponse(images);
        }
    }
}
