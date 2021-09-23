using CommonLayer.UserInterface.Managers;
using CommonLayer.UserInterface.RouteSystem.Routes;
using CommonLayer.UserInterface.Views;

namespace CommonLayer.UserInterface.Presenter
{
    internal interface IPresenterBridge : IRoutedPresenter, IPresenter
    {
        ViewBase View { get; set; }

        IMainUiManager UiManager { get; set; }
    }
}