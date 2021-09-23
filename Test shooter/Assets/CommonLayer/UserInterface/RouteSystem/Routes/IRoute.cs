using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Scenes;
using CommonLayer.UserInterface.RouteSystem.Scenes.Nodes;

namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    public interface IRoute : IReactiveRoute, IRouteInstruction
    {
        INode Node { get; }

        IUiSceneController Scene { get; }

        PresenterBase Presenter { get; }

        bool IsShow { get; }

        string RouteKey { get; }
        bool IsRoot { get; }
        RouteState State { get; }
        bool IsVisible { get; }

        bool HasRoute(string key);

        bool RouteShow();
        bool RouteHide();

        bool IsInState(RouteState routeState);
        bool RouteBackTo(string key, bool autoActivate, out IRoute route);
        bool HasActiveChild(IRoute route);
        
        bool TryGetFirstActiveChild(out IRoute route);
        
        bool TryGetLastActiveChild(out IRoute route);
    }
}