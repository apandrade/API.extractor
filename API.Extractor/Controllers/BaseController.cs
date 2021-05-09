using API.Extractor.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Extractor.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly IControllerService _service;
        protected readonly ILogger<BaseController> _logger;
        protected BaseController(IControllerService service, ILogger<BaseController> logger)
        {
            _logger = logger;
            _service = service;
        }

    }
}
