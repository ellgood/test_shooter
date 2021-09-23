using CommonLayer.UserInterface.RouteSystem.Routes;

namespace CommonLayer.UserInterface.RouteSystem.Scenes
{
    public interface IUiScene : IRouteInstruction
    {
        string SceneTag { get; }
        int Depth { get; }

        void ForceEnd();

        IRoute PeekRoute();

        bool TryPeekRoute(out IRoute route);
    }
}