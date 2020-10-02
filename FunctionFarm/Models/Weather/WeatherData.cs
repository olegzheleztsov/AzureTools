using Newtonsoft.Json;

namespace FunctionFarm.Models.Weather
{
    public class WeatherData
    {
        [JsonProperty("coord")]
        public Coordinate Coordinate { get; set; }
        
        [JsonProperty("base")]
        public string Base { get; set; }
        
        [JsonProperty("main")]
        public WeatherMainData Main { get; set; }
        
        [JsonProperty("visibility")]
        public double Visibility { get; set; }
        
        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }
        
        [JsonProperty("sys")]
        public WeatherSystemData System { get; set; }
        
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("cod")]
        public int Code { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}