using System;

namespace FunctionFarm
{
    public class ConfigurationException : Exception
    {
        public string ConfigurationName { get; }

        public ConfigurationException(string configurationName, string message) : base(message)
        {
            ConfigurationName = configurationName;
        }
    }
}