using Routes.Interfaces;
using Routes.Messages;
using Routes.Models;

namespace Routes.Managers
{
    public class OSRMManager : IOSRMManager
    {
        private readonly HttpClient _httpClient;

        public OSRMManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TimeDistanceTable> GetTimeDistanceTableAsync(List<Coordinate> points)
        {
            if (points == null || points.Count < 2)
            {
                throw new ArgumentException(Errors.PointsListShouldHaveMinimum2Points);
            }

            var coords = string.Join(";", points.Select(p => $"{p.Longitude},{p.Latitude}"));
            string url = $"http://router.project-osrm.org/table/v1/driving/{coords}?annotations=distance,duration";
            var response = await _httpClient.GetFromJsonAsync<TimeDistanceTable>(url);

            if (response == null)
            {
                throw new Exception(Errors.OSRMReturnedEmptyResult);
            }

            return new TimeDistanceTable
            {
                Distances = response.Distances,
                Durations = response.Durations
            };
        }
    }
}
