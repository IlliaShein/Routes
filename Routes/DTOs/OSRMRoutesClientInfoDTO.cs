using Routes.Models;

namespace Routes.DTOs
{
    public class OSRMRoutesClientInfoDTO
    {
        public List<Coordinate> Points { get; set; }
        public int VehicleCount { get; set; }
        public int Iterations { get; set; }
    }
}
