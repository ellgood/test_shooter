using System;

namespace CommonLayer.UserInterface.RouteSystem.Exceptions
{
    public class RouteException : InvalidOperationException
    {
        public RouteException(string message) : base(message) { }
    }
}