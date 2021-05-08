using System;
using System.Threading.Tasks;

namespace API.Extractor.Interfaces
{
    public interface IService
    {
        public Task<IModel> Process(IValueObject vo, Func<object, IModel> createResponse);
    }
}
