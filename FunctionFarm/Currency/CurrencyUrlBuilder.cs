using System;
using FunctionFarm.Configuration;

namespace FunctionFarm.Currency
{
    public class CurrencyUrlBuilder
    {
        private readonly UriBuilder _uriBuilder;

        public CurrencyUrlBuilder(ICurrencyConfiguration configuration, CurrencyKind fromCurrency,
            CurrencyKind toCurrency)
        {
            var fullUrl = configuration.BaseUrl + BuildQueryString(configuration, fromCurrency, toCurrency);
            _uriBuilder = new UriBuilder(fullUrl);
        }

        private string BuildQueryString(ICurrencyConfiguration configuration,
            CurrencyKind fromCurrency, CurrencyKind toCurrency)
        {
            var fromCurrencyStr = fromCurrency.ToString().ToUpperInvariant();
            var toCurrencyStr = toCurrency.ToString().ToUpperInvariant();
            var conversionParam = $"{fromCurrencyStr}_{toCurrencyStr}";
            return $"?q={conversionParam}&compact=ultra&apiKey={configuration.ApiKey}";
        }

        public override string ToString()
        {
            return _uriBuilder.ToString();
        }
    }
}