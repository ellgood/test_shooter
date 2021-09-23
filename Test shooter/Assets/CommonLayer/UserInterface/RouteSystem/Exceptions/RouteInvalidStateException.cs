namespace CommonLayer.UserInterface.RouteSystem.Exceptions
{
    public class RouteInvalidStateException : RouteException
    {
        public RouteInvalidStateException() : this("Route has invalid state") { }

        public RouteInvalidStateException(string message) : base(message) { }
    }
}