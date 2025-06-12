using MathCore.Interfaces;
using MathCore.Models.Results;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class DecompositionsController : ControllerBase
    {
        private readonly IMatrixMapper _mapper;
        private readonly ILogger<DecompositionsController> _logger;

        public DecompositionsController(ILogger<DecompositionsController> logger, IMatrixMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
        /* LUDecomposition, QRDecomposition, CholeskyDecomposition, SVD
          {
              "a": [[0.5, 1.2, 1, 0.9], [1.2, 2, 0.5, 1.2], [1, 0.5, 1, 1], [0.5, 1.2, 1, 2.2]]
          } 
         */

        [HttpPost("LUDecomposition")]
        public ActionResult<TwoMatrixResult> LUDecomposition([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var (L, U) = matrix.LUDecomposition();
            _logger.LogInformation("LUDecomposition matrix operation");
            return Ok(new TwoMatrixResult
            {
                Matrix1Name = "L",
                Matrix2Name = "R",
                Matrix1 = _mapper.ToJagged(L),
                Matrix2 = _mapper.ToJagged(U),
            });
        }

        [HttpPost("QRDecomposition")]
        public ActionResult<TwoMatrixResult> QRDecomposition([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var (Q, R) = matrix.QRDecomposition();
            _logger.LogInformation("QRDecomposition matrix operation");
            return Ok(new TwoMatrixResult
            {
                Matrix1Name = "Q",
                Matrix2Name = "R",
                Matrix1 = _mapper.ToJagged(Q),
                Matrix2 = _mapper.ToJagged(R)
            });
        }

        [HttpPost("CholeskyDecomposition")]
        public ActionResult<TwoMatrixResult> CholeskyDecomposition([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var (L, LT) = matrix.CholeskyDecomposition();
            _logger.LogInformation("CholeskyDecomposition matrix operation");
            return Ok(new TwoMatrixResult
            {
                Matrix1Name = "L",
                Matrix2Name = "LT",
                Matrix1 = _mapper.ToJagged(L),
                Matrix2 = _mapper.ToJagged(LT)
            });
        }

        [HttpPost("SVD")]
        public ActionResult<SVDResult> SVD([FromBody] SingleMatrixDto dto)
        {
            var A = _mapper.FromJagged(dto.A);
            var (U, S, VT) = A.SVD();
            _logger.LogInformation("SVD matrix operation");
            return Ok(new SVDResult
            {
                U = _mapper.ToJagged(U),
                S = _mapper.ToJagged(S),
                VT = _mapper.ToJagged(VT)
            });
        }
    }
}
