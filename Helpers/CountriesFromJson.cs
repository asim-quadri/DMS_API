using ComplianceAPI.Models;
using System.Text.Json;
namespace ComplianceAPI.Helpers
{
    public class CountriesFromJson
    {
        private readonly string _filePath;

        public CountriesFromJson(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Resources", "country-centroids.json");
        }

        public IEnumerable<JsonCountries> GetCountries()
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                var countries = JsonSerializer.Deserialize<List<JsonCountries>>(json);
                return countries ?? new List<JsonCountries>();
            }
            catch(Exception ex)
            {
                return null;
            }            
        }
    }
}
