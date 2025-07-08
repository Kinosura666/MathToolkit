using MathCore.Common;
using MathCore.Interfaces;
using MathCore.Libraries.SortingCore;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers.SortControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SortController : ControllerBase
    {
        private readonly ISortService _service;
        private readonly ILogger<SortController> _logger;

        public SortController(ISortService service, ILogger<SortController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Sort([FromBody] SortRequest request)
        {
            _logger.LogInformation($"{request.algorithm} sort");
            var result = _service.Sort(request);     
            return Ok(result);
        }
    }

}
