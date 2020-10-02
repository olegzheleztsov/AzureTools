using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionFarm
{
    public abstract class WeatherApiUrlBuilder
    {
        private UriBuilder _uriBuilder;

        public WeatherApiUrlBuilder(IWeatherConfiguration weatherConfiguration)
        {
            _uriBuilder = new UriBuilder(weatherConfiguration.BaseApiUrl);
            _uriBuilder.Query = $"?appid={weatherConfiguration.ApiKey}";
        }  
        
        public void AddQueryParam(string key, string value)
        {
            _uriBuilder.Query += $"&{key}={value}";
        }

        public override string ToString()
        {
            return _uriBuilder.ToString();
        }
    }
}
