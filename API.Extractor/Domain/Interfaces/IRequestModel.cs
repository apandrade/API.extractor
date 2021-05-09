using System;

namespace API.Extractor.Domain.Interfaces
{
    public interface IRequestModel
    {
        public IValueObject ConvertToVo(Func<IValueObject> createVo);
    }
}
