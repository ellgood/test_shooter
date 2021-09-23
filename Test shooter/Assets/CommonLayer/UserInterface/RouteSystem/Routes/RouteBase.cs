using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Exceptions;
using CommonLayer.UserInterface.RouteSystem.Scenes;
using CommonLayer.UserInterface.RouteSystem.Scenes.Nodes;
using CommonLayer.UserInterface.Utility.Stateless;
using ModestTree;
using UniRx;

using Debug = UnityEngine.Debug;
using Assert = UnityEngine.Assertions.Assert;
using StackNode = CommonLayer.UserInterface.RouteSystem.Scenes.Nodes.StackNode;

namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    public abstract class RouteBase : IRoute
    {
        public delegate void RouteStateTransition(RouteState from, RouteState to, RouteTriggers byTrigger);

        private readonly StateMachine<RouteState, RouteTriggers> _stateMachine = new StateMachine<RouteState, RouteTriggers>(RouteState.Inactive);

        private readonly ReactiveCommand<RouteState> _stateChanged = new ReactiveCommand<RouteState>();

        private readonly LinkedList<RouteChildRecord> _child = new LinkedList<RouteChildRecord>();

        private LinkedListNode<RouteChildRecord> _parentListNode;

        private readonly StringBuilder _debugSb = new StringBuilder();

        private int _lockedChildState = -1;

        internal RouteBase(ISceneController scene, INode node)
        {
            InternalScene = scene;
            Node = node;

            ConfigureStateMachine(_stateMachine);
        }

        public event RouteStateTransition RouteStateChanged;

        private RouteBase Parent { get; set; }

        private ISceneController InternalScene { get; }

        #region IReactiveRoute Implementation

        public IObservable<RouteState> WhenStateChanged()
        {
            return _stateChanged;
        }

        public IObservable<RouteState> WhenStateEntered(RouteState state)
        {
            return WhenStateChanged()
                .StartWith(_stateMachine.State)
                .Where(s => _stateMachine.IsInState(state));
        }

        public IObservable<RouteState> WhenStateExited(RouteState state)
        {
            return WhenStateChanged()
                .StartWith(_stateMachine.State)
                .Where(s => !_stateMachine.IsInState(state));
        }

        #endregion

        #region IRoute Implementation

        public INode Node { get; }

        public IUiSceneController Scene => InternalScene;
        public abstract PresenterBase Presenter { get; }

        public bool IsShow => IsInState(RouteState.VisibleShow);

        public bool IsVisible => IsInState(RouteState.Visible);

        public string RouteKey => Node.NodeKey;
        public virtual bool IsRoot => false;

        public RouteState State => _stateMachine.State;

        public bool HasRoute(string key)
        {
            return Node.HasChild(key);
        }

        public bool RouteBackTo(string key, bool autoActivate, out IRoute route)
        {
            if (!Node.IsSubNode(key))
            {
                route = null;
                return false;
            }

            route = RouteDownUntil(n => n.NodeKey.Equals(key), autoActivate, true);
            return true;
        }

        public bool RouteShow()
        {
            return Activate();
        }

        public bool RouteHide()
        {
            return Deactivate();
        }

        public bool IsInState(RouteState routeState)
        {
            return _stateMachine.IsInState(routeState);
        }

        public bool HasActiveChild(string key)
        {
            return _child.Any(c => c.Child.RouteKey == key);
        }

        public bool HasActiveChild(IRoute route)
        {
            return _child.Any(c => c.Child.Equals(route));
        }

        public bool TryGetFirstActiveChild(out IRoute route)
        {
            if (_child.Count <= 0)
            {
                route = default;
                return false;
            }

            route = _child.First.Value.Child;
            return true;
        }

        public bool TryGetLastActiveChild(out IRoute route)
        {
            if (_child.Count <= 0)
            {
                route = default;
                return false;
            }

            route = _child.Last.Value.Child;
            return true;
        }

        #endregion

        #region IRouteInstruction Implementation

        public bool TryRouteTo(string key, out IRoute transition)
        {
            if (Node.TryGetNode(key, out INode node))
            {
                transition = RouteUp(node);
                return true;
            }

            transition = null;
            return false;
        }

        public IRoute RouteTo(string key, bool activate = true)
        {
            INode transitionNode = Node.GetNode(key);

            return RouteUp(transitionNode, activate);
        }

        public bool RouteBack(bool force, bool autoActivate, out IRoute previousRoute)
        {
            if (Parent == null)
            {
                previousRoute = null;
                return false;
            }

            RouteDown(force, autoActivate, out RouteBase parent);
            previousRoute = parent;
            return true;
        }

        public IRoute RouteBackRoot(bool force)
        {
            return RouteDownUntil(node => node is RootNode, true, force);
        }

        #endregion

        public override string ToString()
        {
            return $"{RouteKey}:{GetType().PrettyName()}";
        }

        public bool IsSubRouteBy(string routeKey)
        {
            return Node.IsSubNode(routeKey);
        }

        protected abstract void OnStart();

        protected abstract void OnShown();

        protected abstract void OnHidden();

        protected abstract void OnEndState();

        protected abstract void OnHiding();

        protected abstract void OnShowing();

        protected void SetActivated()
        {
            CheckChildState();
        }

        protected void SetDeactivated()
        {
            CheckChildState();
        }

        internal void Initialize(bool initActiveState)
        {
            if (_stateMachine.IsInState(RouteState.Inactive))
            {
                _stateMachine.Fire(initActiveState ? RouteTriggers.InitShowed : RouteTriggers.InitHidden);
            }
            else
            {
                throw new RouteInvalidStateException("Route can't start with any state when already initialized");
            }
        }

        internal void Close(bool force)
        {
            RouteTriggers targetTrigger = force ? RouteTriggers.ForceClose : RouteTriggers.Close;

            if (_stateMachine.CanFire(targetTrigger))
            {
                _stateMachine.Fire(targetTrigger);
            }
            else
            {
                throw new RouteInvalidStateException($"Route<{Presenter?.GetType().Name}> can't use trigger {targetTrigger} from state {_stateMachine.State}");
            }
        }

        private bool Activate()
        {
            if (!_stateMachine.CanFire(RouteTriggers.Show))
            {
                return false;
            }

            _stateMachine.Fire(RouteTriggers.Show);
            return true;
        }

        private bool Deactivate()
        {
            if (!_stateMachine.CanFire(RouteTriggers.Hide))
            {
                return false;
            }

            _stateMachine.Fire(RouteTriggers.Hide);
            return true;
        }

        private void RouteDown(bool force, bool activatePrevious, out RouteBase parent)
        {
            parent = Parent;

            if (parent != null)
            {
                BreakRelationship();
            }

            if (Node is SubStackNode)
            {
                Close(force);
            }

            if (!(Node is StackNode))
            {
                return;
            }

            RouteBase currentStackHead = InternalScene.Pop();

            if (!currentStackHead.Equals(this))
            {
                throw new RouteInvalidStateException("This route its not active head of stack");
            }

            Close(force);

            if (!activatePrevious || InternalScene.Depth <= 0)
            {
                return;
            }

            RouteBase peek = InternalScene.Peek();
            if (!peek.IsShow)
            {
                peek.Activate();
            }
        }

        private void ConfigureStateMachine(StateMachine<RouteState, RouteTriggers> stateMachine)
        {
            stateMachine.OnUnhandledTrigger(OnUnhandledTriggerAction);
            stateMachine.OnTransitioned(OnTransitionAction);

            stateMachine.Configure(RouteState.Inactive)
                .Permit(RouteTriggers.InitShowed, RouteState.Showing)
                .Permit(RouteTriggers.InitHidden, RouteState.Hidden);

            stateMachine.Configure(RouteState.Opened)
                .Permit(RouteTriggers.ForceClose, RouteState.Disposed)
                .OnEntry(OnStart);

            stateMachine.Configure(RouteState.Visible)
                .SubstateOf(RouteState.Opened);

            stateMachine.Configure(RouteState.VisibleShow)
                .SubstateOf(RouteState.Visible)
                .Permit(RouteTriggers.Close, RouteState.Closing);

            stateMachine.Configure(RouteState.VisibleHide)
                .SubstateOf(RouteState.Visible);

            stateMachine.Configure(RouteState.Showing)
                .SubstateOf(RouteState.VisibleShow)
                .Permit(RouteTriggers.Showed, RouteState.Shown)
                .OnEntry(OnShowing);

            stateMachine.Configure(RouteState.Shown)
                .SubstateOf(RouteState.VisibleShow)
                .Permit(RouteTriggers.Hide, RouteState.Hiding)
                .Permit(RouteTriggers.HideByParent, RouteState.ParentHiding)
                .OnEntry(OnShown);

            stateMachine.Configure(RouteState.Hiding)
                .SubstateOf(RouteState.VisibleHide)
                .Permit(RouteTriggers.Close, RouteState.Closing)
                .Permit(RouteTriggers.HidingComplete, RouteState.Hidden)
                .OnEntry(OnHiding);

            stateMachine.Configure(RouteState.Closing)
                .SubstateOf(RouteState.Hiding)
                .Permit(RouteTriggers.HidingComplete, RouteState.Disposed);

            stateMachine.Configure(RouteState.Hidden)
                .SubstateOf(RouteState.Opened)
                .Permit(RouteTriggers.Show, RouteState.Showing)
                .Permit(RouteTriggers.Close, RouteState.Disposed)
                .OnEntry(OnHidden);

            stateMachine.Configure(RouteState.ParentHiding)
                .SubstateOf(RouteState.Hiding)
                .Permit(RouteTriggers.HidingComplete, RouteState.ParentHidden);

            stateMachine.Configure(RouteState.ParentHidden)
                .SubstateOf(RouteState.Hidden)
                .Permit(RouteTriggers.Show, RouteState.Showing);

            stateMachine.Configure(RouteState.Disposed)
                .OnEntryFrom(RouteTriggers.HidingComplete, OnHidden)
                .OnEntry(OnEndState);
        }

        private void OnTransitionAction(StateMachine<RouteState, RouteTriggers>.Transition obj)
        {
            _lockedChildState = 0;
            RouteStateChanged?.Invoke(obj.Source, obj.Destination, obj.Trigger);
            _stateChanged.Execute(obj.Destination);
            if (_lockedChildState != 0)
            {
                CheckChildState();
            }

            _lockedChildState = -1;

            Debug.Log($"Route {this} was changed state from {obj.Source} to {obj.Destination} by trigger {obj.Trigger}");
        }

        private void OnUnhandledTriggerAction(RouteState state, RouteTriggers trigger)
        {
            Debug.Log($"Route {this} was fired by unhandled trigger {trigger} in state {state}");
        }

        private RouteBase RouteDownUntil(Predicate<INode> predicate, bool autoActivate = false, bool force = false)
        {
            if (predicate.Invoke(Node))
            {
                Assert.IsFalse(IsInState(RouteState.Disposed));

                if (Node is StackNode)
                {
                    if (!IsInState(RouteState.VisibleShow) && autoActivate)
                    {
                        Activate();
                    }
                }
                else
                {
                    RouteBase currentStackNode = InternalScene.Peek();
                    if (!currentStackNode.IsInState(RouteState.VisibleShow))
                    {
                        currentStackNode.Activate();
                    }
                }

                return this;
            }

            RouteDown(force, false, out RouteBase parent);
            return parent?.RouteDownUntil(predicate);
        }

        private IRoute RouteRecursion(INode node, bool checkChild = false)
        {
            RouteBase recursionRoute = RouteDownUntil(n => n.IsSameNode(node) || checkChild && n.HasChild(node));

            if (recursionRoute.Node is StackNode && recursionRoute.IsInState(RouteState.VisibleShow))
            {
                recursionRoute.Deactivate();
            }

            return recursionRoute.RouteUp(node);
        }

        private IRoute RouteUp(INode node, bool activate = true)
        {
            RouteBase route = node.CreateRoute();

            if (node is StackNode)
            {
                if (InternalScene.Depth > 0)
                {
                    InternalScene.Peek().Deactivate();
                }

                InternalScene.Push(route);
            }

            route.Initialize(activate);

            IDisposable subDisposable = route.WhenStateChanged().Subscribe(OnChildStateChanged);
            LinkedListNode<RouteChildRecord> childContainer = _child.AddLast(new RouteChildRecord(route, subDisposable));

            route.SetParentInfo(childContainer, this);

            return route;
        }

        private void OnChildStateChanged(RouteState state)
        {
            if (_lockedChildState != -1)
            {
                _lockedChildState++;
                return;
            }

            Debug.Log($"OnChildStateChanged<{Presenter?.GetType().Name}>");
            CheckChildState();
        }

        [Conditional("DEBUG")]
        private void AppendDebug(string debugLine)
        {
            _debugSb.AppendLine(debugLine);
        }

        [Conditional("DEBUG")]
        private void FlushDebug()
        {
            Debug.Log(_debugSb.ToString());
            _debugSb.Clear();
        }

        private void CheckChildState()
        {
            AppendDebug($"CheckChildState<{Presenter?.GetType().Name}>");

            if (_stateMachine.IsInState(RouteState.Showing))
            {
                var readyToTransition = true;
                foreach (RouteChildRecord routeChildRecord in _child)
                {
                    RouteBase childRoute = routeChildRecord.Child;
                    AppendDebug($"Child <{childRoute.Presenter.GetType().Name}> state: {childRoute.State}");
                    //Waiting processes
                    if (!childRoute.IsInState(RouteState.Showing) && !childRoute.IsInState(RouteState.Hiding))
                    {
                        continue;
                    }

                    readyToTransition = false;
                    break;
                }

                if (readyToTransition)
                {
                    _stateMachine.Fire(RouteTriggers.Showed);
                }

                FlushDebug();
            }
            else if (_stateMachine.IsInState(RouteState.Hiding))
            {
                var readyToTransition = true;
                foreach (RouteChildRecord routeChildRecord in _child)
                {
                    RouteBase childRoute = routeChildRecord.Child;
                    //Waiting processes
                    if (!childRoute.IsInState(RouteState.Showing) && !childRoute.IsInState(RouteState.Hiding))
                    {
                        continue;
                    }

                    readyToTransition = false;
                    break;
                }

                if (readyToTransition)
                {
                    _stateMachine.Fire(RouteTriggers.HidingComplete);
                }
            }
        }

        private void SetParentInfo(LinkedListNode<RouteChildRecord> linkedListNode, RouteBase parent)
        {
            _parentListNode = linkedListNode;
            Parent = parent;

            Parent.RouteStateChanged += OnParentRouteStateChanged;
        }

        private void BreakRelationship()
        {
            if (Parent == null)
            {
                return;
            }

            Parent.RouteStateChanged -= OnParentRouteStateChanged;

            //Break subscription
            _parentListNode.Value.Relations.Dispose();
            //Remove from child linked list
            _parentListNode.List.Remove(_parentListNode);

            Parent = null;
        }

        private void OnParentRouteStateChanged(RouteState from, RouteState to, RouteTriggers trigger)
        {
            Debug.Log($"From {from} to {to} trigger {trigger} Node: [{Node.NodeKey}] Parent: [{Node.Parent?.NodeKey}]");

            if (!(Node is SubStackNode))
            {
                return;
            }

            switch (trigger)
            {
                case RouteTriggers.Close:
                case RouteTriggers.ForceClose:
                    BreakRelationship();
                    Close(true);

                    break;
                case RouteTriggers.HideByParent:
                case RouteTriggers.Hide:
                    _stateMachine.Fire(RouteTriggers.HideByParent);
                    break;
                case RouteTriggers.Show when IsInState(RouteState.ParentHidden):
                    _stateMachine.Fire(RouteTriggers.Show);
                    break;
            }
        }

        public struct RouteChildRecord
        {
            public readonly RouteBase Child;
            public readonly IDisposable Relations;

            public RouteChildRecord(RouteBase child, IDisposable relations)
            {
                Child = child;
                Relations = relations;
            }
        }
    }
}