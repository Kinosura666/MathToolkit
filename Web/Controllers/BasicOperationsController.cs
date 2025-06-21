using Microsoft.AspNetCore.Mvc;
using Web.Models;
using MathCore.Models;
using MathCore.Mappers;
using MathCore.Interfaces;
using MathCore.Libraries.MatrixCore;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class BasicOperationsController : ControllerBase
    {
        private readonly IMatrixMapper _mapper;
        private readonly ILogger<BasicOperationsController> _logger;
        public BasicOperationsController(IMatrixMapper mapper, ILogger<BasicOperationsController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        /* Add, Subtract, Multiply, Transpose, Inverse, Determinant, PseudoInverse, Power, Symmetrize, Rank
          {
              "a": [[0.5, 1.2, 1, 0.9], [1.2, 2, 0.5, 1.2], [1, 0.5, 1, 1], [0.5, 1.2, 1, 2.2]],
              "b": [[0.5, 1.2, 1, 0.9], [1.2, 2, 0.5, 1.2], [1, 0.5, 1, 1], [0.5, 1.2, 1, 2.2]]
          } 
         */

        [HttpPost("Add")]
        public ActionResult<double[][]> Add([FromBody] MatrixPairDto dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var matrixB = _mapper.FromJagged(dto.B);
            var result = MatrixOperations.Add(matrixA, matrixB);
            _logger.LogInformation("Add matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Subtract")]
        public ActionResult<double[][]> Subtract([FromBody] MatrixPairDto dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var matrixB = _mapper.FromJagged(dto.B);
            var result = MatrixOperations.Subtract(matrixA, matrixB);
            _logger.LogInformation("Subtract matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Multiply")]
        public ActionResult<double[][]> Multiply([FromBody] MatrixPairDto dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var matrixB = _mapper.FromJagged(dto.B);
            var result = MatrixOperations.Multiply(matrixA, matrixB);
            _logger.LogInformation("Multiply matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Transpose")]
        public ActionResult<double[][]> Transpose([FromBody] SingleMatrixDto dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var result = MatrixOperations.Transpose(matrixA);
            _logger.LogInformation("Transpose matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Inverse")]
        public ActionResult<double[][]> Inverse([FromBody] SingleMatrixDto dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var result = MatrixOperations.Inverse(matrixA);
            _logger.LogInformation("Inverse matrix operation");
            return Ok(_mapper.ToJagged(result));

        }

        [HttpPost("Determinant")]
        public ActionResult<double[][]> Determinant([FromBody] SingleMatrixDto dto)
        {

            var matrixA = _mapper.FromJagged(dto.A);
            var result = MatrixOperations.Determinant(matrixA);
            _logger.LogInformation("Determinant matrix operation");
            return Ok(result);

        }

        [HttpPost("PseudoInverse")]
        public ActionResult<double[][]> PseudoInverse([FromBody] SingleMatrixDto dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var result = MatrixOperations.PseudoInverse(matrixA);
            _logger.LogInformation("PseudoInverse matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Power")]
        public ActionResult<double[][]> Power([FromBody] PowerMatrix dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var result = MatrixOperations.Power(matrixA, dto.exponent);
            _logger.LogInformation("Power matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Symmetrize")]
        public ActionResult<double[][]> Symmetrize([FromBody] SingleMatrixDto dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var result = MatrixOperations.Symmetrize(matrixA);
            _logger.LogInformation("Symmetrize matrix operation");
            return Ok(_mapper.ToJagged(result));
        }

        [HttpPost("Rank")]
        public ActionResult<double[][]> Rank([FromBody] SingleMatrixDto dto)
        {
            var matrixA = _mapper.FromJagged(dto.A);
            var result = MatrixOperations.Rank(matrixA);
            _logger.LogInformation("Rank matrix operation");
            return Ok(result);
        }
    }
}
