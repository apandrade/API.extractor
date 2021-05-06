using API.Extractor.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Interfaces
{
    public interface IRequestModel
    {
        public IValueObject ConvertToVo(Func<IValueObject> createVo);
    }
}
