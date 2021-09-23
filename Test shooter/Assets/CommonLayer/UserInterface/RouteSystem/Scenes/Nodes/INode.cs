using System;
using CommonLayer.UserInterface.RouteSystem.Routes;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public interface INode : INodeConfigurator
    {
        string NodeKey { get; }

        INode Parent { get; }

        bool IsSameNode(INode node);

        bool HasChild(string key, Predicate<INode> predicate);

        bool HasChild(string key);

        bool HasChild(INode node);

        INode GetNode(string key);

        bool TryGetNode(string key, out INode node);

        RouteBase CreateRoute();

        bool IsRepresent<TRoutePresenter>();

        bool IsSubNode(string key);
    }
}