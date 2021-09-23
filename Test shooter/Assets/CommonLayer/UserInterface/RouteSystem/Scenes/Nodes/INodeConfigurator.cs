using CommonLayer.UserInterface.Presenter;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public interface INodeConfigurator : IConfigurator
    {
        INode MakeRecursive();

        INode ToSub<TPresenter>(string key)
            where TPresenter : PresenterBase;
    }
}