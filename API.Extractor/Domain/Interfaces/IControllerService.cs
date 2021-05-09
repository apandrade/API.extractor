﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Domain.Interfaces
{
    public interface IControllerService : IService
    {
        public Task<IResponseModel> Process(IValueObject vo, Func<object, object, IResponseModel> createResponse);
    }
}
