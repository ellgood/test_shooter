using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.Views;
using UnityEngine;
using Zenject;

namespace CommonLayer.UserInterface.Managers
{
    public interface IMainUiManager
    {
        Rect CanvasRect { get; }

        T Create<T>(DiContainer container)
            where T : PresenterBase;

        T Create<T>(DiContainer container, string viewKey)
            where T : PresenterBase;

        T Create<T>(DiContainer container, string viewKey, params object[] args)
            where T : PresenterBase;

        ViewBase RentView(string viewKey);
        void ReturnView(ViewBase view);
    }
}