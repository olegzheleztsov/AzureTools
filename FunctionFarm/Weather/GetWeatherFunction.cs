using System.Threading.Tasks;
using FunctionFarm.Configuration;
using FunctionFarm.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionFarm.Weather
{
    public static class GetWeatherFunction
    {
        //Api used from https://openweathermap.org/
        [FunctionName("GetWeatherFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            if (!req.Query.ContainsKey("city")) return new BadRequestResult();

            string city = req.Query["city"];
            var configurationFactory = new ConfigurationFactory();
            var configuration = configurationFactory.GetWeatherConfiguration(log);
            var service = new WeatherService(configuration);
            var data = await service.GetWeatherAsync(city).ConfigureAwait(false);
            log.LogInformation(data.ToString());
            return new OkObjectResult(data);
        }
    }
}