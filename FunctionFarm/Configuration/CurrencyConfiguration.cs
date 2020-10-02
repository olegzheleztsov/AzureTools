using Newtonsoft.Json;

namespace FunctionFarm.Configuration
{
    public class CurrencyConfiguration : ICurrencyConfiguration
    {
        public CurrencyConfiguration(string baseUrl, string apiKey)
        {
            BaseUrl = baseUrl;
            ApiKey = apiKey;
        }
        public string BaseUrl { get; }
        public string ApiKey { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}