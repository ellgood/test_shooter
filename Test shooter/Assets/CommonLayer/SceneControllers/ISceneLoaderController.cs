namespace CommonLayer.SceneControllers
{
    internal interface ISceneLoaderController : ISceneLoader
    {
        void SetCurrentSceneController(SceneControllerBase sceneController);
    }
}