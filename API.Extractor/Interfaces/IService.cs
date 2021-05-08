using System;
using System.Threading.Tasks;

namespace API.Extractor.Interfaces
{
    public interface IService
    {
        public Task<IResponseModel> Process(IValueObject vo, Func<object, IResponseModel> createResponse);
    }
}
