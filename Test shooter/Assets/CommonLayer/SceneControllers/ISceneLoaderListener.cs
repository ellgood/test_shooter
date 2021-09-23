namespace CommonLayer.SceneControllers
{
    public interface ISceneLoaderListener
    {
        void StartSceneLoading(string loadingSceneName);

        void EndSceneLoading(string loadingSceneName);

        void SceneLoadingProgress(string loadingSceneName, float progress);
    }
}