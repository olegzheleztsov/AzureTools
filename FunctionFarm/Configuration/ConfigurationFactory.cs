using System;
using Microsoft.Extensions.Logging;

namespace FunctionFarm.Configuration
{
    public class ConfigurationFactory
    {
        public WeatherConfiguration GetWeatherConfiguration(ILogger logger)
        {
            var url = Environment.GetEnvironmentVariable("WeatherAppUrl");
            var appId = Environment.GetEnvironmentVariable("WeatherAppId");

            if (string.IsNullOrEmpty(url))
                throw new ConfigurationException("WeatherAppUrl", Constants.CONFIG_MISSED_ERROR_MESSAGE);

            if (string.IsNullOrEmpty(appId))
                throw new ConfigurationException("WeatherAppId", Constants.CONFIG_MISSED_ERROR_MESSAGE);

            var config = new WeatherConfiguration(url, appId);
            logger.LogInformation(config.ToString());
            return config;
        }

        public ICurrencyConfiguration GetCurrencyConfiguration()
        {
            var baseUrl = Environment.GetEnvironmentVariable("CurrencyBaseUrl");
            var appId = Environment.GetEnvironmentVariable("CurrencyAppId");
            if (string.IsNullOrEmpty(baseUrl))
                throw new ConfigurationException("CurrencyBaseUrl", Constants.CONFIG_MISSED_ERROR_MESSAGE);

            if (string.IsNullOrEmpty(appId))
                throw new ConfigurationException("CurrencyAppId", Constants.CONFIG_MISSED_ERROR_MESSAGE);
            return new CurrencyConfiguration(baseUrl, appId);
        }
    }
}