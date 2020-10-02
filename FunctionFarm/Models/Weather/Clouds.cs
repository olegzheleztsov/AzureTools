using Newtonsoft.Json;

namespace FunctionFarm.Models.Weather
{
    public class Clouds
    {
        [JsonProperty("all")]
        public double All { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}