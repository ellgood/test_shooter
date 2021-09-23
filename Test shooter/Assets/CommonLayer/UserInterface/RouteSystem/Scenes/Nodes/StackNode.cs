namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public abstract class StackNode : NodeBase
    {
        internal StackNode(string routeKey, ISceneController scene, NodeBase parentNode) : base(routeKey, scene, parentNode) { }
    }
}