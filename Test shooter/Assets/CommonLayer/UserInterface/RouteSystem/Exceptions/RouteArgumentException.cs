using System;

namespace CommonLayer.UserInterface.RouteSystem.Exceptions
{
    public sealed class RouteArgumentException : ArgumentException
    {
        public RouteArgumentException(string paramName, string message) : base(message, paramName) { }
    }
}