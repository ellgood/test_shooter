
using CommonLayer.SceneControllers;
using Zenject;

namespace MenuLayer
{
    public class MenuSceneInstaller : MonoInstaller<MenuSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindSceneController<MenuSceneController>();
        }
    }
}