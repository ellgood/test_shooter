namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    public static class RouteExtensions
    {
        public static IRoute RouteBack(this IRoute route, bool force = false, bool activatePrevious = true)
        {
            return route.RouteBack(force, activatePrevious, out IRoute backRoute) ? backRoute : null;
        }
    }
}