using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionFarm
{
    public interface IWeatherConfiguration
    {
        string BaseApiUrl { get; }
        string ApiKey { get; }
    }
}
