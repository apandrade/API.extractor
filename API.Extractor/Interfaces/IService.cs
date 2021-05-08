using API.Extractor.Models;
using API.Extractor.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Interfaces
{
    public interface IService
    {
        public Task<IResponseModel> Process(IValueObject vo, Func<object,IResponseModel> createResponse);
    }
}
