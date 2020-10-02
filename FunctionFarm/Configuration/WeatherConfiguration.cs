using Newtonsoft.Json;

namespace FunctionFarm.Configuration
{
    public class WeatherConfiguration : IWeatherConfiguration
    {
        public WeatherConfiguration(string baseUrl, string apiKey)
        {
            BaseApiUrl = baseUrl;
            ApiKey = apiKey;
        }

        public string BaseApiUrl { get; }

        public string ApiKey { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
