using FunctionFarm.Configuration;
using FunctionFarm.Currency;
using Moq;
using Xunit;

namespace FunctionFarm.Tests.Currency
{
    public class CurrencyUrlBuilderTests
    {
        [Fact]
        public void Should_Build_Valid_Url()
        {
            Mock<ICurrencyConfiguration> config = new Mock<ICurrencyConfiguration>();
            config.SetupGet(c => c.ApiKey).Returns("1234");
            config.SetupGet(c => c.BaseUrl).Returns("https://google.com");
            
            CurrencyUrlBuilder builder = new CurrencyUrlBuilder(config.Object, CurrencyKind.Uah, CurrencyKind.Usd);
            var result = builder.ToString();
            Assert.Equal("https://google.com:443/?q=UAH_USD&compact=ultra&apiKey=1234", result);
        }
    }
}