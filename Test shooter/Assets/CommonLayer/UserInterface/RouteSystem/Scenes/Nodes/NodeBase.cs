using System;
using System.Collections.Generic;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Exceptions;
using CommonLayer.UserInterface.RouteSystem.Routes;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public abstract class NodeBase : INode
    {
        private readonly NodeBase _parent;

        internal NodeBase(string routeKey, ISceneController scene, NodeBase parentNode, IDictionary<string, NodeBase> innerTransitions)
        {
            InnerTransitions = innerTransitions;
            _parent = parentNode;
            HasParent = _parent != null;

            NodeKey = routeKey;
            Scene = scene;
        }

        internal NodeBase(string routeKey, ISceneController scene, NodeBase parentNode)
            : this(routeKey, scene, parentNode,
                new Dictionary<string, NodeBase>()) { }

        public bool HasParent { get; }

        internal ISceneController Scene { get; }

        internal IDictionary<string, NodeBase> InnerTransitions { get; }

        #region IConfigurator Implementation

        public INode AddRoute<TPresenter>(string routeKey)
            where TPresenter : PresenterBase
        {
            if (InnerTransitions.ContainsKey(routeKey))
            {
                throw new RouteArgumentException(nameof(routeKey), $"Already exist route with key {routeKey}");
            }

            var route = new PresenterNode<TPresenter>(routeKey, Scene, this);
            InnerTransitions.Add(routeKey, route);

            return route;
        }

        #endregion

        #region INode Implementation

        public string NodeKey { get; }
        public INode Parent => _parent;

        public bool HasChild(INode node)
        {
            return InnerTransitions.TryGetValue(node.NodeKey, out NodeBase child) && child.IsSameNode(node);
        }

        public INode GetNode(string key)
        {
            if (InnerTransitions.TryGetValue(key, out NodeBase node))
            {
                return node;
            }

            throw new KeyNotFoundException($"Key {key} not found in node {NodeKey}");
        }

        public bool TryGetNode(string key, out INode node)
        {
            if (InnerTransitions.TryGetValue(key, out NodeBase nodeBase))
            {
                node = nodeBase;
                return true;
            }

            node = null;
            return false;
        }

        public virtual RouteBase CreateRoute()
        {
            return CreateRoute(this);
        }

        public abstract bool IsRepresent<TRoutePresenter>();

        public bool IsSubNode(string key)
        {
            return NodeKey.Equals(key) || HasParent && _parent.IsSubNode(key);
        }

        public virtual bool IsSameNode(INode node)
        {
            return node.NodeKey == NodeKey;
        }

        public bool HasChild(string key, Predicate<INode> predicate)
        {
            return TryGetNode(key, out INode transition) && predicate(transition);
        }

        public bool HasChild(string key)
        {
            return InnerTransitions.ContainsKey(key);
        }

        #endregion

        #region INodeConfigurator Implementation

        public INode MakeRecursive()
        {
            if (InnerTransitions.ContainsKey(NodeKey))
            {
                return this;
            }

            var recursive = new RecursiveCallNode(NodeKey, Scene, this);
            InnerTransitions.Add(NodeKey, recursive);

            return this;
        }

        public INode ToSub<TPresenter>(string key)
            where TPresenter : PresenterBase
        {
            if (InnerTransitions.ContainsKey(key) || key.Equals(NodeKey))
            {
                throw new RouteArgumentException(nameof(key), $"This name \"{key}\" is already reserved");
            }

            var route = new SubPresenterNode<TPresenter>(key, Scene, this);
            InnerTransitions.Add(key, route);

            return route;
        }

        #endregion

        public override string ToString()
        {
            return $"{NodeKey}";
        }

        internal abstract RouteBase CreateRoute(NodeBase based);
    }
}