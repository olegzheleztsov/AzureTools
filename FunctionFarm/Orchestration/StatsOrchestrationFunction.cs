using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FunctionFarm.Configuration;
using FunctionFarm.Currency;
using FunctionFarm.Models.Weather;
using FunctionFarm.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionFarm.Orchestration
{
    public static class StatsOrchestrationFunction
    {
        [FunctionName("StatsOrchestrationFunction")]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<GetMethodInput>();
            if (input == null) throw new ArgumentNullException(nameof(input));

            var weather = await context
                .CallActivityAsync<WeatherData>("StatsOrchestrationFunction_GetWeather", input.City)
                .ConfigureAwait(false);
            var currency = await context
                .CallActivityAsync<CurrencyConversionResult>("StatsOrchestrationFunction_GetCurrencyConversion", input)
                .ConfigureAwait(false);
            return JsonConvert.SerializeObject(new
            {
                Weather = weather,
                Currency = currency
            });
        }

        [FunctionName("StatsOrchestrationFunction_GetWeather")]
        public static async Task<WeatherData> GetWeatherAsync([ActivityTrigger] string city, ILogger log)
        {
            log.LogInformation("Getting weather...");
            var configurationFactory = new ConfigurationFactory();
            var service = new WeatherService(configurationFactory.GetWeatherConfiguration(log));
            var weatherData = await service.GetWeatherAsync(city).ConfigureAwait(false);
            return weatherData;
        }

        [FunctionName("StatsOrchestrationFunction_GetCurrencyConversion")]
        public static async Task<CurrencyConversionResult> GetCurrencyConversionAsync(
            [ActivityTrigger] GetMethodInput input, ILogger log)
        {
            log.LogInformation("Getting currency conversion...");
            var configurationFactory = new ConfigurationFactory();
            var service = new CurrencyService(configurationFactory.GetCurrencyConfiguration());
            var result = await service.GetCurrencyAsync(input.FromKind, input.ToKind).ConfigureAwait(false);
            return result;
        }

        [FunctionName("StatsOrchestrationFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
            HttpRequestMessage req,
            IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            var instanceId = await starter.StartNewAsync("StatsOrchestrationFunction", null, ParseInput(req));

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        private static GetMethodInput ParseInput(HttpRequestMessage requestMessage)
        {
            var queryParameters = requestMessage.RequestUri.ParseQueryString();
            var dict = queryParameters.Cast<KeyValuePair<string, string>>()
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            if (!dict.ContainsKey("city")) throw new ArgumentException("city");

            if (!dict.ContainsKey("from")) throw new ArgumentException("from");

            if (!dict.ContainsKey("to")) throw new ArgumentException("to");

            var city = dict["city"];
            var fromKind = dict["from"];
            var toKind = dict["to"];
            var kindConverter = new CurrencyKindConverter();
            return new GetMethodInput
            {
                City = city,
                FromKind = kindConverter.Convert(fromKind),
                ToKind = kindConverter.Convert(toKind)
            };
        }
    }

    public class GetMethodInput
    {
        public CurrencyKind FromKind { get; set; }
        public CurrencyKind ToKind { get; set; }

        public string City { get; set; }
    }
}