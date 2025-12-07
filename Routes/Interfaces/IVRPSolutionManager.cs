using Routes.Models;

namespace Routes.Interfaces
{
    public interface IVRPSolutionManager
    {
        VRPSolution SolveVRP(double[][] distances, int vehicleCount, int iterations);
    }

}
