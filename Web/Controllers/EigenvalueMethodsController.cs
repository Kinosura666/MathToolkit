using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class EigenvalueMethodsController : ControllerBase
    {
        private readonly ILogger _logger;

        public EigenvalueMethodsController(ILogger logger)
        {
            _logger = logger;
        }

        /* PowerIteration, InversePowerIteration, RayleighQuotientIteration, GershgorinDiscs, 
        JacobiEigenSolver, QREigenValues, LREigenValues, LeverrierFaddeev, KrylovCharacteristicPolynomial 
          {
              "a": [[0.5, 1.2, 1, 0.9], [1.2, 2, 0.5, 1.2], [1, 0.5, 1, 1], [0.5, 1.2, 1, 2.2]]
          } 
         */
    }
}
