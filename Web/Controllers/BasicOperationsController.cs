using Microsoft.AspNetCore.Mvc;
using Web.Models;
using MathCore.Models;
using Web.Mappers;
using Web.Interfaces;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class BasicOperationsController : ControllerBase
    {
        private readonly IMatrixMapper _mapper;
        private readonly ILogger _logger;
        public BasicOperationsController(IMatrixMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("Add")]
        public ActionResult<double[][]> Add([FromBody] MatrixPairDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var B = _mapper.FromJagged(dto.B);
            var result = A.Add(B);
            _logger.LogInformation("Add matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Subtract")]
        public ActionResult<double[][]> Subtract([FromBody] MatrixPairDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var B = _mapper.FromJagged(dto.B);
            var result = A.Subtract(B);
            _logger.LogInformation("Subtract matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Multiply")]
        public ActionResult<double[][]> Multiply([FromBody] MatrixPairDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var B = _mapper.FromJagged(dto.B);
            var result = A.Multiply(B);
            _logger.LogInformation("Multiply matrix operation");
            return Ok(_mapper.ToJagged(result));
        }
        [HttpPost("Transpose")]
        public ActionResult<double[][]> Transpose([FromBody] SingleMatrixDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var result = A.Transpose();
            _logger.LogInformation("Transpose matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Inverse")]
        public ActionResult<double[][]> Inverse([FromBody] SingleMatrixDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var result = A.Inverse();
            _logger.LogInformation("Inverse matrix operation");
            return Ok(_mapper.ToJagged(result));

        }

        [HttpPost("Determinant")]
        public ActionResult<double[][]> Determinant([FromBody] SingleMatrixDto dto)
        {

            var A = _mapper.FromJagged(dto.A);
            var result = A.Determinant();
            _logger.LogInformation("Determinant matrix operation");
            return Ok(result);

        }

        [HttpPost("PseudoInverse")]
        public ActionResult<double[][]> PseudoInverse([FromBody] SingleMatrixDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var result = A.PseudoInverse();
            _logger.LogInformation("PseudoInverse matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Power")]
        public ActionResult<double[][]> Power([FromBody] PowerMatrix dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var result = A.Power(dto.exponent);
            _logger.LogInformation("Power matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Symmetrize")]
        public ActionResult<double[][]> Symmetrize([FromBody] SingleMatrixDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var result = A.Symmetrize();
            _logger.LogInformation("Symmetrize matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Rank")]
        public ActionResult<double[][]> Rank([FromBody] SingleMatrixDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var result = A.Rank();
            _logger.LogInformation("Rank matrix operation");
            return Ok(result);
        }
    }
}
