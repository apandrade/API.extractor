using API.Extractor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Controllers
{
    public abstract class BaseController:ControllerBase
    {
        protected readonly IService _service;
        protected readonly ILogger<BaseController> _logger;
        protected BaseController(IService service, ILogger<BaseController> logger)
        {
            _logger = logger;
            _service = service;
        }

    }
}
