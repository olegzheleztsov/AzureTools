namespace FunctionFarm.Currency
{
    public class CurrencyConversionResult
    {
        public CurrencyKind FromCurrency { get; }
        public CurrencyKind ToCurrency { get;  }
        public double Rate { get; }

        public CurrencyConversionResult(CurrencyKind fromCurrency, CurrencyKind toCurrency, double rate)
        {
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            Rate = rate;
        }
    }
}