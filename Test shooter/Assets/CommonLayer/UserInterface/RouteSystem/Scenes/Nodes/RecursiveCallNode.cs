using CommonLayer.UserInterface.RouteSystem.Routes;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public sealed class RecursiveCallNode : SubStackNode
    {
        private readonly NodeBase _mirrorNode;

        internal RecursiveCallNode(string routeKey, ISceneController scene, NodeBase mirrorNode) : base(
            routeKey, scene, mirrorNode, mirrorNode.InnerTransitions)
        {
            _mirrorNode = mirrorNode;
        }

        public override bool IsRepresent<TRoutePresenter>()
        {
            return _mirrorNode.IsRepresent<TRoutePresenter>();
        }

        public override bool IsSameNode(INode node)
        {
            bool nodeEquality = _mirrorNode.IsSameNode(node);
            bool mirrorEquality = node is RecursiveCallNode recursiveNode && _mirrorNode.IsSameNode(recursiveNode._mirrorNode);

            return nodeEquality || mirrorEquality;
            ;
        }

        internal override RouteBase CreateRoute(NodeBase based)
        {
            return _mirrorNode.CreateRoute(this);
        }
    }
}