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

        public SortController(ISortService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Sort([FromBody] SortRequest request)
        {
            var result = _service.Sort(request);
            return Ok(result);
        }
    }

}
