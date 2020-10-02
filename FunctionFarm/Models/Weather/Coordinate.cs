using Newtonsoft.Json;

namespace FunctionFarm.Models.Weather
{
    public class Coordinate
    {
        [JsonProperty("lon")]
        public double Longitude { get; set; }
        
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}