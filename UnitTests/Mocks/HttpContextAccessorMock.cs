using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Mocks
{
    public class HttpContextAccessorMock : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }
    }
}
