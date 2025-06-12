using MathCore.Models.Results;
using Microsoft.AspNetCore.Mvc;
using MathCore.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class EigenvalueMethodsController : ControllerBase
    {
        private readonly ILogger<EigenvalueMethodsController> _logger;
        private readonly IMatrixMapper _mapper;

        public EigenvalueMethodsController(ILogger<EigenvalueMethodsController> logger, IMatrixMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /* PowerIteration, InversePowerIteration, RayleighQuotientIteration, GershgorinDiscs, 
        JacobiEigenSolver, QREigenValues, LREigenValues, LeverrierFaddeev, KrylovCharacteristicPolynomial 
          {
              "a": [[0.5, 1.2, 1, 0.9], [1.2, 2, 0.5, 1.2], [1, 0.5, 1, 1], [0.5, 1.2, 1, 2.2]]
          } 
         */

        [HttpPost("PowerIteration")]
        public ActionResult<EigenIterationResult> PowerIteration([FromBody]SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.PowerIteration();
            _logger.LogInformation("PowerIteration matrix operation");
            return Ok(result);
        }

        [HttpPost("InversePowerIteration")]
        public ActionResult<EigenIterationResult> InversePowerIteration([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.InversePowerIteration();
            _logger.LogInformation("InversePowerIteration matrix operation");
            return Ok(result);
        }

        [HttpPost("RayleighQuotientIteration")]
        public ActionResult<EigenIterationResult> RayleighQuotientIteration([FromBody]SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.RayleighQuotientIteration();
            _logger.LogInformation("RayleighQuotientIteration matrix operation");
            return Ok(result);
        }

        [HttpPost("GershgorinDiscs")]
        public ActionResult<GershgorinDiscsResult> GershgorinDiscs([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.GershgorinDiscs();
            _logger.LogInformation("GershgorinDiscs matrix operation");
            return Ok(result);
        }

        [HttpPost("JacobiEigenSolver")]
        public ActionResult<JacobiEigenResult> JacobiEigenSolver([FromBody]SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.JacobiEigenSolver();
            _logger.LogInformation("JacobiEigenSolver matrix operation");
            return Ok(result);
        }

        [HttpPost("QREigenValues")]
        public ActionResult<EigenvalueListResult> QREigenValues([FromBody]SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.QREigenValues();
            _logger.LogInformation("QREigenValues matrix operation");
            return Ok(result);
        }

        [HttpPost("LREigenValues")]
        public ActionResult<EigenvalueListResult> LREigenValues([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.LREigenValues();
            _logger.LogInformation("LREigenValues matrix operation");
            return Ok(result);
        }

        [HttpPost("LeverrierFaddeev")]
        public ActionResult<CharacteristicPolynomialResult> LeverrierFaddeev([FromBody]SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.LeverrierFaddeev();
            _logger.LogInformation("LeverrierFaddeev matrix operation");
            return Ok(result);
        }

        [HttpPost("KrylovCharacteristicPolynomial")]
        public ActionResult<CharacteristicPolynomialResult> KrylovCharacteristicPolynomial([FromBody] SingleMatrixDto dto)
        {
            var matrix = _mapper.FromJagged(dto.A);
            var result = matrix.KrylovCharacteristicPolynomial();
            _logger.LogInformation("KrylovCharacteristicPolynomial matrix operation");
            return Ok(result);
        }
    }
}
