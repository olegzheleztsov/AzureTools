using System;

namespace FunctionFarm
{
    public class ServiceException : Exception
    {
        public ServiceException(string message): base(message) {}
    }
}