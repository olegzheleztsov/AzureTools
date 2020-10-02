using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FunctionFarm.Configuration;
using FunctionFarm.Models.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionFarm.Weather
{
    public static class GetWeatherFunction
    {
        [FunctionName("GetWeatherFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            if (!req.Query.ContainsKey("city"))
            {
                return new BadRequestResult();
            }

            string city = req.Query["city"];
            var urlBuilder = new CityWeatherApiUrlBuilder(GetConfiguration(log), city);
            urlBuilder.UseTemperatureUnits(TemperatureUnits.Celsius);
            using var client = new HttpClient();
            var response = await client.GetAsync(urlBuilder.ToString()).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var data = JsonConvert.DeserializeObject<WeatherData>(result);
            log.LogInformation(data.ToString());
            return new OkObjectResult(data);
        }

        private static WeatherConfiguration GetConfiguration(ILogger logger)
        {
            var url = Environment.GetEnvironmentVariable("WeatherAppUrl");
            var appId = Environment.GetEnvironmentVariable("WeatherAppId");

            if (string.IsNullOrEmpty(url))
            {
                throw new ConfigurationException("WeatherAppUrl", Constants.CONFIG_MISSED_ERROR_MESSAGE);
            }

            if (string.IsNullOrEmpty(appId))
            {
                throw new ConfigurationException("WeatherAppId",Constants.CONFIG_MISSED_ERROR_MESSAGE);
            }
            
            var config =  new WeatherConfiguration(url, appId);
            logger.LogInformation(config.ToString());
            return config;
        }
    }
}
