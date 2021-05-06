using API.Extractor.Interfaces;
using API.Extractor.Models.Request;
using API.Extractor.Models.Response;
using API.Extractor.VO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extractor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtractController : BaseController
    {

        public ExtractController(IService service, ILogger<ExtractController> logger):base(service,logger)
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExtractorRequest model)
        {
            if (!model.IsValid())
                return BadRequest("Invalid URL");

            _logger.LogInformation($"The url received is {model.Url}");
            IValueObject vo = model.ConvertToVo(() => new Website(model.Url));
            IResponseModel response = await _service.Process(vo);
            return new JsonResult(response);
        }
    }
}
