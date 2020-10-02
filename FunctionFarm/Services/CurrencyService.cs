using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using FunctionFarm.Configuration;
using FunctionFarm.Currency;
using Newtonsoft.Json;

namespace FunctionFarm.Services
{
    public class CurrencyService
    {
        private readonly ICurrencyConfiguration _configuration;

        public CurrencyService(ICurrencyConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CurrencyConversionResult> GetCurrencyAsync(CurrencyKind fromKind, CurrencyKind toKind)
        {
            using var client = new HttpClient();
            var urlBuilder = new CurrencyUrlBuilder(_configuration, fromKind, toKind);
            var response = await client.GetAsync(urlBuilder.ToString()).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return ParseResult(result, fromKind, toKind);
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