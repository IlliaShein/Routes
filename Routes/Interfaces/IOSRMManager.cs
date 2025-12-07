using Routes.Models;

namespace Routes.Interfaces
{
    public interface IOSRMManager
    {
        Task<TimeDistanceTable> GetTimeDistanceTableAsync(List<Coordinate> points);
    }
}
