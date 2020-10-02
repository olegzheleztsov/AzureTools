namespace FunctionFarm.Configuration
{
    public interface IWeatherConfiguration
    {
        string BaseApiUrl { get; }
        string ApiKey { get; }
    }
}