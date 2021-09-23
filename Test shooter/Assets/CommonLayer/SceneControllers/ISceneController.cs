using CommonLayer.UserInterface.Managers;
using Zenject;

namespace CommonLayer.SceneControllers
{
    public interface ISceneController
    {
        bool IsLoaded { get; }

        DiContainer SceneContainer { get; }
        IUiManager UiManager { get; }
    }
}