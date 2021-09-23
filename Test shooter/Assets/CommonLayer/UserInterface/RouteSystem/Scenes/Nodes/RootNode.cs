using CommonLayer.UserInterface.RouteSystem.Routes;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public sealed class RootNode : StackNode
    {
        internal RootNode(string routeKey, ISceneController scene) : base(routeKey, scene, null) { }

        public override bool IsRepresent<TRoutePresenter>()
        {
            return false;
        }

        public override bool IsSameNode(INode node)
        {
            return base.IsSameNode(node) && node is RootNode;
        }

        internal override RouteBase CreateRoute(NodeBase based)
        {
            return new RootRoute(Scene, based);
        }
    }
}