using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Routes;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public sealed class PresenterNode<TRoutePresenter> : StackNode
        where TRoutePresenter : PresenterBase
    {
        internal PresenterNode(string routeKey, ISceneController scene, NodeBase parent) : base(routeKey, scene, parent) { }

        public override bool IsRepresent<TPresenter>()
        {
            return typeof(TRoutePresenter) == typeof(TPresenter);
        }

        public override bool IsSameNode(INode node)
        {
            return base.IsSameNode(node) && node is PresenterNode<TRoutePresenter>;
        }

        public override string ToString()
        {
            return $"{NodeKey}:{typeof(TRoutePresenter).Name}";
        }

        internal override RouteBase CreateRoute(NodeBase based)
        {
            return new PresenterRoute<TRoutePresenter>(Scene, based);
        }
    }
}