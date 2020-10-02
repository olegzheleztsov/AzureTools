
using FunctionFarm.Configuration;

namespace FunctionFarm.Weather
{
    public class CityWeatherApiUrlBuilder : WeatherApiUrlBuilder
    {
        public CityWeatherApiUrlBuilder(IWeatherConfiguration weatherConfiguration, string city) : 
            base(weatherConfiguration)
        {
            AddQueryParam("q", city);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
