using Moq;
using Xunit;

namespace FunctionFarm.Tests
{
    public class CityWeatherApiUrlBuilderTests
    {
        [Fact]
        public void Should_Generate_Expected_Url()
        {
            var configMock = new Mock<IWeatherConfiguration>();
            configMock.SetupGet(obj => obj.BaseApiUrl).Returns("http://google.com");
            configMock.SetupGet(obj => obj.ApiKey).Returns("12345");
            var builder = new CityWeatherApiUrlBuilder(configMock.Object, "London");
            var actualUrl = builder.ToString();
            Assert.Equal("http://google.com:80/?appid=12345&q=London", actualUrl);
        }
    }
}
