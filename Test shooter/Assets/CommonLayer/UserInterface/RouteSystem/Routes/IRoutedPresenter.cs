using System;

namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    internal interface IRoutedPresenter
    {
        IRoute Route { set; }

        void RouteInitialize();

        void RouteClose();

        void RouteHide(Action onHidden);

        void RouteShow(Action onShowed);

        void RouteShowed();

        void RouteHidden();
    }
}