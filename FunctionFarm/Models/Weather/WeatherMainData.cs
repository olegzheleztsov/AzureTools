using Newtonsoft.Json;

namespace FunctionFarm.Models.Weather
{
    public class WeatherMainData
    {
        [JsonProperty("temp")]
        public double Temperature { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLikeTemperature { get; set; }
        
        [JsonProperty("temp_min")]
        public double TemperatureMin { get; set; }
        
        [JsonProperty("temp_max")]
        public double TemperatureMax { get; set; }

        [JsonProperty("pressure")]
        public double Pressure { get; set; }

        [JsonProperty("humidity")]
        public double Humidity { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}