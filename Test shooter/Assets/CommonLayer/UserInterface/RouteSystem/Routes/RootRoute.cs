using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Scenes;
using CommonLayer.UserInterface.RouteSystem.Scenes.Nodes;

namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    internal sealed class RootRoute : RouteBase
    {
        public RootRoute(ISceneController scene, INode rootNode) : base(scene, rootNode) { }

        public override PresenterBase Presenter => null;

        public override bool IsRoot => true;

        protected override void OnStart() { }

        protected override void OnShown() { }

        protected override void OnHidden() { }

        protected override void OnEndState() { }

        protected override void OnHiding() { }

        protected override void OnShowing() { }
    }
}