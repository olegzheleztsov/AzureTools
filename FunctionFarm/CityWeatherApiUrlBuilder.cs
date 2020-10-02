
namespace FunctionFarm
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
