using System.Net.Http;
using System.Threading.Tasks;
using FunctionFarm.Configuration;
using FunctionFarm.Models.Weather;
using FunctionFarm.Weather;
using Newtonsoft.Json;

namespace FunctionFarm.Services
{
    public class WeatherService
    {
        private readonly IWeatherConfiguration _configuration;

        public WeatherService(IWeatherConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            using var client = new HttpClient();
            var urlBuilder =
                new CityWeatherApiUrlBuilder(_configuration, city).UseTemperatureUnits(TemperatureUnits.Celsius);
            var response = await client.GetAsync(urlBuilder.ToString()).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var data = JsonConvert.DeserializeObject<WeatherData>(result);
            return data;
        }
    }
}