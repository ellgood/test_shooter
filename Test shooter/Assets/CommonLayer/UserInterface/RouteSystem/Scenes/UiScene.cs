using System;
using System.Collections.Generic;
using CommonLayer.UserInterface.Managers;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Exceptions;
using CommonLayer.UserInterface.RouteSystem.Routes;
using CommonLayer.UserInterface.RouteSystem.Scenes.Nodes;

namespace CommonLayer.UserInterface.RouteSystem.Scenes
{
    internal sealed class UiScene : ISceneController
    {
        private readonly IUiManager _uiManager;
        private readonly Action<UiScene> _endCallback;

        private readonly RootNode _rootNode;
        private readonly RouteBase _startRoute;

        private readonly Stack<RouteBase> _routesStack;

        public UiScene(string sceneTag, IUiManager uiManager, Action<UiScene> endCallback)
        {
            SceneTag = sceneTag;
            _uiManager = uiManager;
            _endCallback = endCallback;
            _uiManager.Cleaning += OnUiManagerCleaned;

            _rootNode = new RootNode(sceneTag, this);
            _startRoute = _rootNode.CreateRoute();
            _routesStack = new Stack<RouteBase>();
            _routesStack.Push(_startRoute);

            _startRoute.Initialize(true);
        }

        private IRoute CurrentRoute => PeekRoute();

        #region IConfigurator Implementation

        public INode AddRoute<TPresenter>(string routeKey)
            where TPresenter : PresenterBase
        {
            return _rootNode.AddRoute<TPresenter>(routeKey);
        }

        #endregion

        #region IRouteInstruction Implementation

        public bool TryRouteTo(string key, out IRoute transition)
        {
            return CurrentRoute.TryRouteTo(key, out transition);
        }

        public IRoute RouteTo(string key, bool show = true)
        {
            if (Depth > 0)
            {
                return _routesStack.Peek().RouteTo(key, show);
            }

            throw new RouteException("Ui scene stack is empty");
        }

        public bool RouteBack(bool force, bool autoActivate, out IRoute previousRoute)
        {
            return CurrentRoute.RouteBack(force, autoActivate, out previousRoute);
        }

        public IRoute RouteBackRoot(bool force)
        {
            return CurrentRoute.RouteBackRoot(force);
        }

        #endregion

        #region ISceneController Implementation

        RouteBase ISceneController.Peek()
        {
            return _routesStack.Peek();
        }

        RouteBase ISceneController.Pop()
        {
            if (Depth == 1)
            {
                throw new InvalidOperationException("Can't remove root route");
            }

            return _routesStack.Pop();
        }

        void ISceneController.Push(RouteBase route)
        {
            _routesStack.Push(route);
        }

        T ISceneController.CreatePresenter<T>(string viewKey)
        {
            return _uiManager.Create<T>(viewKey);
        }

        #endregion

        #region IUiScene Implementation

        public int Depth => _routesStack.Count;

        public string SceneTag { get; }

        public void ForceEnd()
        {
            ForceSceneEndInternal(true);
        }

        public IRoute PeekRoute()
        {
            return _routesStack.Peek();
        }

        public bool TryPeekRoute(out IRoute route)
        {
            if (_routesStack.Count > 0)
            {
                route = _routesStack.Peek();
                return true;
            }

            route = null;
            return false;
        }

        #endregion

        public IRoute RouteBack(bool showPrevious = true)
        {
            if (_routesStack.Peek().RouteBack(false, true, out IRoute parent))
            {
                return parent;
            }

#if DEBUG
            throw new RouteException("Ui scene stack is empty");
#else
            return null;
#endif
        }

        private void OnUiManagerCleaned()
        {
            ForceSceneEndInternal(false);
        }

        private void ForceSceneEndInternal(bool withCallback)
        {
            _uiManager.Cleaning -= OnUiManagerCleaned;

            _startRoute.Close(true);

            if (withCallback)
            {
                _endCallback?.Invoke(this);
            }
        }
    }
}