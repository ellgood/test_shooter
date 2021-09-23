
using System.Diagnostics;
using System.Text;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Scenes;
using CommonLayer.UserInterface.RouteSystem.Scenes.Nodes;
using Debug = UnityEngine.Debug;

namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    public sealed class PresenterRoute<TPresenter> : RouteBase
        where TPresenter : PresenterBase
    {
        private readonly ISceneController _scene;

        private PresenterBase _presenter;

        private string _presenterName;

        internal PresenterRoute(ISceneController scene, INode node) : base(scene, node)
        {
            _scene = scene;
        }

        public override PresenterBase Presenter => _presenter;

        private IRoutedPresenter Routed => _presenter;

        protected override void OnStart()
        {
            _presenter = CreatePresenter();
            DebugEnterState(nameof(OnStart));

            Routed.Route = this;
            Routed.RouteInitialize();
            ExitState(nameof(OnStart));
        }

        protected override void OnShowing()
        {
            DebugEnterState(nameof(OnShowing));
            Routed.RouteShow(SetActivated);
            ExitState(nameof(OnShowing));
        }

        protected override void OnHiding()
        {
            DebugEnterState(nameof(OnHiding));
            Routed.RouteHide(SetDeactivated);
            ExitState(nameof(OnHiding));
        }

        protected override void OnShown()
        {
            DebugEnterState(nameof(OnShown));
            Routed.RouteShowed();
            ExitState(nameof(OnShown));
        }

        protected override void OnHidden()
        {
            DebugEnterState(nameof(OnHidden));
            Routed.RouteHidden();
            ExitState(nameof(OnHidden));
        }

        protected override void OnEndState()
        {
            DebugEnterState(nameof(OnEndState));
            Routed.RouteClose();
            Routed.Route = null;
            ExitState(nameof(OnEndState));
        }

        [Conditional("DEBUG")]
        private void GetPresenterName(StringBuilder sb)
        {
            if (_presenter != null)
            {
                sb.Append(_presenterName ??= _presenter.GetType().Name);
            }
            else
            {
                sb.Append(string.IsNullOrWhiteSpace(_presenterName) ? "Null" : $"Null({_presenterName})");
            }
        }

        [Conditional("DEBUG")]
        private void DebugEnterState(string methodName)
        {
            StringBuilder sb = PresenterRouteUtils.DebugStringBuilder;
            sb.Append("Presenter<");
            GetPresenterName(sb);
            sb.Append("> enter ");
            sb.Append(methodName);

            Debug.Log(sb.ToString());
        }

        [Conditional("DEBUG")]
        private void ExitState(string methodName)
        {
            StringBuilder sb = PresenterRouteUtils.DebugStringBuilder;
            sb.Append("Presenter<");
            GetPresenterName(sb);
            sb.Append("> exit ");
            sb.Append(methodName);

            Debug.Log(sb.ToString());
        }

        private PresenterBase CreatePresenter()
        {
            return _scene.CreatePresenter<TPresenter>(null);
        }
    }
}