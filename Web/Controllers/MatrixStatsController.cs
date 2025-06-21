using Microsoft.AspNetCore.Mvc;
using MathCore.Interfaces;
using Web.Models;
using MathCore.Libraries.MatrixCore;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class MatrixStatsController : ControllerBase
    {
        private readonly IMatrixMapper _mapper;
        private readonly ILogger<MatrixStatsController> _logger;
        public MatrixStatsController(IMatrixMapper mapper, ILogger<MatrixStatsController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        /* FrobeniusNorm, InfinityNorm, OneNorm, TwoNorm, ConditionNumber2, GetSingularValues 
          {
              "a": [[0.5, 1.2, 1, 0.9], [1.2, 2, 0.5, 1.2], [1, 0.5, 1, 1], [0.5, 1.2, 1, 2.2]]
          } 
         */


        [HttpPost("FrobeniusNorm")]
        public ActionResult<double> FrobeniusNorm([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = MatrixStats.FrobeniusNorm(matrix);
            _logger.LogInformation("FrobeniusNorm matrix operation");
            return Ok(result);
        }

        [HttpPost("InfinityNorm")]
        public ActionResult<double> InfinityNorm([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = MatrixStats.InfinityNorm(matrix);
            _logger.LogInformation("InfinityNorm matrix operation");
            return Ok(result);
        }

        [HttpPost("OneNorm")]
        public ActionResult<double> OneNorm([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = MatrixStats.OneNorm(matrix);
            _logger.LogInformation("OneNorm matrix operation");
            return Ok(result);
        }

        [HttpPost("TwoNorm")]
        public ActionResult<double> TwoNorm([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = MatrixStats.TwoNorm(matrix);
            _logger.LogInformation("TwoNorm matrix operation");
            return Ok(result);
        }

        [HttpPost("ConditionNumber2")]
        public ActionResult<double> ConditionNumber2([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = MatrixStats.ConditionNumber2(matrix);
            _logger.LogInformation("ConditionNumber2 matrix operation");
            return Ok(result);
        }

        [HttpPost("GetSingularValues")]
        public ActionResult<double[]> GetSingularValues([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = MatrixStats.GetSingularValues(matrix);
            _logger.LogInformation("GetSingularValues matrix operation");
            return Ok(result);
        }
    }
}
