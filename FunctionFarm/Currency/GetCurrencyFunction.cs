using System;
using System.Threading.Tasks;
using System.Web.Http;
using FunctionFarm.Configuration;
using FunctionFarm.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

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

                var configurationFactory = new ConfigurationFactory();
                var service = new CurrencyService(configurationFactory.GetCurrencyConfiguration());
                var result = await service.GetCurrencyAsync(fromKind, toKind).ConfigureAwait(false);
                log.LogInformation(result.ToString());
                return new OkObjectResult(result);
            }
            catch (Exception exception)
            {
                return new ExceptionResult(exception, true);
            }
        }
    }
}