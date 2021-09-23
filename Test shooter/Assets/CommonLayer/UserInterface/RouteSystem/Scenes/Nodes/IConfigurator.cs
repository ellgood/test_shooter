using CommonLayer.UserInterface.Presenter;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public interface IConfigurator
    {
        INode AddRoute<TPresenter>(string routeKey)
            where TPresenter : PresenterBase;
    }
}