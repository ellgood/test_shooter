using CommonLayer.UserInterface.RouteSystem.Scenes.Nodes;

namespace CommonLayer.UserInterface.RouteSystem.Exceptions
{
    public class RouteUnactivatedException : RouteException
    {
        public RouteUnactivatedException(INode uiNode) : base($"Unactivated route exception: {uiNode}") { }
    }
}