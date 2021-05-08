using API.Extractor.Interfaces;
using API.Extractor.Models.Request;
using API.Extractor.Models.Response;
using API.Extractor.VO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Extractor.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExtractorController : BaseController
    {

        public ExtractorController(IService service, ILogger<ExtractorController> logger) : base(service, logger)
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
            IValueObject vo = model.ConvertToVo(() => new WebsiteVO { Url = model.Url, Download = model.Download });
            _logger.LogInformation($"Processing URL: {model.Url}");
            IModel response = await _service.Process(vo, (result) => new ExtractorResponse((IList<IValueObject>)result));
            string jsonString = JsonConvert.SerializeObject(response);
            _logger.LogInformation($"Response: {jsonString}");

            return new JsonResult(response);
        }
    }
}
