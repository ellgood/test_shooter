using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace CommonLayer.SceneControllers
{
    public interface ISceneLoader 
    {
        event SceneLoaderEvent SceneLoaded;
        event SceneLoaderEvent SceneUnload;

        event SceneLoaderEvent ScenePostLoadCallback;
        event SceneLoaderEvent SceneUnLoadCallback;

        ISceneController SceneController { get; }

        Scene CurrentScene { get; }

        string ViewLoadingKey { get; }

        bool InLoading { get; }

        void LoadScene(string sceneName, DiContainer[] parentContainer = null);
        void LoadScene(string sceneName, string[] decoratorScenes, string[] additiveScenes, DiContainer[] parentContainers = null);

        IDisposable Report(ISceneLoaderListener listener);
    }
}