using System.Collections.Generic;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public abstract class SubStackNode : NodeBase
    {
        internal SubStackNode(string routeKey, ISceneController scene, NodeBase parentNode, IDictionary<string, NodeBase> innerTransitions) : base(
            routeKey, scene, parentNode, innerTransitions) { }

        internal SubStackNode(string routeKey, ISceneController scene, NodeBase parentNode) : base(routeKey, scene, parentNode) { }
    }
}