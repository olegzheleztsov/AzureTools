using System;
using System.Collections.Generic;

namespace FunctionFarm
{
    public static class WeatherUrlBuilderExtensions
    {
        private static readonly Dictionary<TemperatureUnits, string> TemperatureUnitNames =
            new Dictionary<TemperatureUnits, string>()
            {
                [TemperatureUnits.Fahrenheit] = "imperial",
                [TemperatureUnits.Celsius] = "metric",
                [TemperatureUnits.Kelvin] = "standard"
            };
        
        public static WeatherApiUrlBuilder UseTemperatureUnits(this WeatherApiUrlBuilder builder,
            TemperatureUnits units)
        {
            if (!TemperatureUnitNames.ContainsKey(units))
            {
                throw new ArgumentException(nameof(units));
            }

            var unitName = TemperatureUnitNames[units];
            builder.AddQueryParam("units", unitName);
            return builder;
        }
    }
}