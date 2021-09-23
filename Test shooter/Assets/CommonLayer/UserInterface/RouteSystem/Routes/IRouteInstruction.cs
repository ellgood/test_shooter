namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    public interface IRouteInstruction
    {
        bool TryRouteTo(string key, out IRoute transition);
        IRoute RouteTo(string key, bool activate = true);
        bool RouteBack(bool force, bool autoActivate, out IRoute previousRoute);
        IRoute RouteBackRoot(bool force);
    }
}