using ComplianceAPI.Models;
using System.Text.Json;

namespace ComplianceAPI.Helpers
{
    public class GetCoordinates : IGetCoordinates
    {
        public async Task<EntitiesCityCoordinate?> GetCoordinatesFromCityAsync(string city)
        {
            var apiKey = "AIzaSyDqgmTHErJiPKYeofoDLBZjQAvSa8MG72E"; // Replace with your actual API key
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(city)}&key={apiKey}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);

            var status = data.RootElement.GetProperty("status").GetString();
            if (status != "OK")
                return null;

            var location = data.RootElement
                .GetProperty("results")[0]
                .GetProperty("geometry")
                .GetProperty("location");

            return new EntitiesCityCoordinate
            {
                Latitude = location.GetProperty("lat").GetDouble(),
                Longitude = location.GetProperty("lng").GetDouble()
            };
        }
    }
}

