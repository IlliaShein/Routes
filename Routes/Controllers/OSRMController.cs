using Microsoft.AspNetCore.Mvc;
using Routes.DTOs;
using Routes.Interfaces;
using Routes.Messages;

namespace Routes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OSRMController : ControllerBase
    {
        private IOSRMManager _oSRMManager;
        private readonly IVRPSolutionManager _vRPSolutionManager;

        public OSRMController(IOSRMManager oSRMManager, IVRPSolutionManager vRPSolutionManager)
        {
            _oSRMManager = oSRMManager;
            _vRPSolutionManager = vRPSolutionManager;
        }

        [HttpPost("table")]
        public async Task<IActionResult> CreateRoutesSolution([FromBody] OSRMRoutesClientInfoDTO info)
        {
            if (info == null || info.Points == null || info.Points.Count <= 2)
            {
                return BadRequest(Errors.PointsListShouldHaveMinimum2Points);
            }

            if (info.VehicleCount <= 0)
            {
                return BadRequest(Errors.VehiclesCountShouldBeAtLeast1);
            }

            var timeDistanceTable = await _oSRMManager.GetTimeDistanceTableAsync(info.Points);
            var result = _vRPSolutionManager.SolveVRP(timeDistanceTable.Durations, info.VehicleCount, info.Iterations);

            return Ok(result);
        }
    }
}
