using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Routes;

namespace CommonLayer.UserInterface.RouteSystem.Scenes
{
    internal interface ISceneController : IUiSceneController
    {
        RouteBase Peek();

        RouteBase Pop();

        void Push(RouteBase route);

        T CreatePresenter<T>(string viewKey)
            where T : PresenterBase;
    }
}