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
    public class ExtractorController : BaseController
    {

        public ExtractorController(IService service, ILogger<ExtractorController> logger):base(service,logger)
        {
            
        }

        /// <summary>
        /// Extract all images and rankign top 10 most used words from a given URL
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "Url": "https://www.altudo.co",
        ///     }
        ///
        /// </remarks>
        /// <param name="url">Valid URL string</param>
        /// <response code="400">If the URL is invalid</response>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ExtractorRequest model)
        {
            _logger.LogInformation($"The url received is {model.Url}");
            IValueObject vo = model.ConvertToVo(() => new Website(model.Url));
            IResponseModel response = await _service.Process(vo);
            return new JsonResult(response);
        }
    }
}
