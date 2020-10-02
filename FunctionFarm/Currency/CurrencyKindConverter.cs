using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;

namespace FunctionFarm.Currency
{
    public class CurrencyKindConverter : IConverter<string, CurrencyKind>
    {
        private static readonly Dictionary<string, CurrencyKind> ConversionMap = new Dictionary<string, CurrencyKind>
        {
            ["usd"] = CurrencyKind.Usd,
            ["uah"] = CurrencyKind.Uah
        };

        public CurrencyKind Convert(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentException(nameof(input));
            if (!ConversionMap.ContainsKey(input.ToLowerInvariant())) throw new ArgumentException(nameof(input));
            return ConversionMap[input.ToLowerInvariant()];
        }
    }
}