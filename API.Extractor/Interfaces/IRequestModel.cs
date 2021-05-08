using System;

namespace API.Extractor.Interfaces
{
    public interface IRequestModel
    {
        public IValueObject ConvertToVo(Func<IValueObject> createVo);
    }
}
