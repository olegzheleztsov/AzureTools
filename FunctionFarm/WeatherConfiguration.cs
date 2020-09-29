using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionFarm
{
    public class WeatherConfiguration : IWeatherConfiguration
    {
        public string BaseApiUrl => Config.BaseWeatherApiUrl;

        public string ApiKey => Config.WeatherApiKey;
    }
}
