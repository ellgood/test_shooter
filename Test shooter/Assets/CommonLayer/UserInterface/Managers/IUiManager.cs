using System;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Scenes;

namespace CommonLayer.UserInterface.Managers
{
    public interface IUiManager : IDisposable
    {
        event Action Cleaning;

        T Create<T>()
            where T : PresenterBase;

        T Create<T>(string view)
            where T : PresenterBase;

        T Create<T>(string view, params object[] args)
            where T : PresenterBase;

        IUiSceneController CreateUiScene(string sceneKey);

        bool TryGetScene(string sceneKey, out IUiSceneController uiScene);
    }
}