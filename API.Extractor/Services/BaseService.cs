using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Services
{
    public abstract class BaseService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public HttpContext CurrentContext { get => _httpContextAccessor.HttpContext; }
        public BaseService(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
        }
        public string AppBaseUrl { get => $"{CurrentContext.Request.Scheme}://{CurrentContext.Request.Host}{CurrentContext.Request.PathBase}";  }
    }
}
