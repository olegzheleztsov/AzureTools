using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FunctionFarm.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionFarm.Currency
{
    public static class GetCurrencyFunction
    {
        [FunctionName("GetCurrencyFunction")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            if (!req.Query.ContainsKey("from")) return new BadRequestResult();

            if (!req.Query.ContainsKey("to")) return new BadRequestResult();

            try
            {
                string fromCurrency = req.Query["from"];
                string toCurrency = req.Query["to"];
                var currencyConverter = new CurrencyKindConverter();
                var fromKind = currencyConverter.Convert(fromCurrency);
                var toKind = currencyConverter.Convert(toCurrency);

                var client = new HttpClient();

                var urlBuilder = new CurrencyUrlBuilder(GetConfiguration(), fromKind, toKind);
                var response = await client.GetAsync(urlBuilder.ToString()).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                log.LogInformation(result);
                return new OkObjectResult(ParseResult(result, fromKind, toKind));
            }
            catch (Exception exception)
            {
                return new ExceptionResult(exception, true);
            }
        }

        private static ICurrencyConfiguration GetConfiguration()
        {
            var baseUrl = Environment.GetEnvironmentVariable("CurrencyBaseUrl");
            var appId = Environment.GetEnvironmentVariable("CurrencyAppId");
            if (string.IsNullOrEmpty(baseUrl))
                throw new ConfigurationException("CurrencyBaseUrl", Constants.CONFIG_MISSED_ERROR_MESSAGE);

            if (string.IsNullOrEmpty(appId))
                throw new ConfigurationException("CurrencyAppId", Constants.CONFIG_MISSED_ERROR_MESSAGE);
            return new CurrencyConfiguration(baseUrl, appId);
        }

        private static CurrencyConversionResult ParseResult(string result, CurrencyKind fromCurrency,
            CurrencyKind toCurrency)
        {
            var resultDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            var requiredKey =
                $"{fromCurrency.ToString().ToUpperInvariant()}_{toCurrency.ToString().ToUpperInvariant()}";
            if (!resultDictionary.ContainsKey(requiredKey))
                throw new ServiceException($"Result doesn't contain key: {requiredKey}");

            if (!double.TryParse(resultDictionary[requiredKey], NumberStyles.Any, CultureInfo.InvariantCulture,
                out var rate))
                throw new ServiceException($"Fail to convert rate to double: {resultDictionary[requiredKey]}");
            return new CurrencyConversionResult(fromCurrency, toCurrency, rate);
        }
    }
}