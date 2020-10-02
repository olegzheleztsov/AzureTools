using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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
            log.LogInformation(result);
            return new OkObjectResult(result);
        }

        private static WeatherConfiguration GetConfiguration(ILogger logger)
        {
            var url = Environment.GetEnvironmentVariable("WeatherAppUrl");
            var appId = Environment.GetEnvironmentVariable("WeatherAppId");

            if (string.IsNullOrEmpty(url))
            {
                throw new ConfigurationException("WeatherAppUrl", "Environment variable is missed");
            }

            if (string.IsNullOrEmpty(appId))
            {
                throw new ConfigurationException("WeatherAppId","Environment variable is missed");
            }
            
            var config =  new WeatherConfiguration(url, appId);
            logger.LogInformation(config.ToString());
            return config;
        }
    }
}
