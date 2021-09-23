using Zenject;

namespace CommonLayer.SceneControllers
{
    public sealed class SceneControllersInstaller : Installer<SceneControllersInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().ToSelf().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().FromResolve();
            Container.Bind<IInitializable>().To<SceneLoader>().FromResolve();
            Container.Bind<ISceneLoaderController>().To<SceneLoader>().FromResolve();
        }
    }
}