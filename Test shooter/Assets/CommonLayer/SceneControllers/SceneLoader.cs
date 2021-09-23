using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Interface;
using CommonLayer.SceneControllers.Routines;
using CommonLayer.UserInterface.Managers;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Routes;
using CommonLayer.UserInterface.RouteSystem.Scenes;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Zenject;

namespace CommonLayer.SceneControllers
{
    public sealed class SceneLoader : ISceneLoaderController,IInitializable
    {
        private readonly IUiManager _uiManager;
        private SceneControllerBase _currentSceneController;

        private readonly ManualRoutineDispatcher _routineDispatcher;

        private readonly LinkedList<ISceneLoaderListener> _listenerList = new LinkedList<ISceneLoaderListener>();

        private Scene _currentScene;

        private readonly List<Scene> _decoratorScenes = new List<Scene>();
        private readonly List<Scene> _additiveScenes = new List<Scene>();
        private readonly SceneLoaderConfiguration _config;
        private IUiSceneController _uiScene;
        private IRoute _route;

        public SceneLoader(IResourcesController resourcesController, IUiManager uiManager)
        {
            _uiManager = uiManager;
            _config = resourcesController.GetData<SceneLoaderConfiguration>();
            _routineDispatcher = new ManualRoutineDispatcher();
        }

        #region ISceneLoader Implementation

        public event SceneLoaderEvent SceneLoaded;
        public event SceneLoaderEvent SceneUnload;
        
        public event SceneLoaderEvent ScenePostLoadCallback;
        public event SceneLoaderEvent SceneUnLoadCallback;

        public Scene CurrentScene => _currentScene;

        public string ViewLoadingKey => _config.Content.ViewLoadingKey;

        public bool InLoading { get; private set; }

        public ISceneController SceneController => _currentSceneController;

        //TODO: Make possible recursion load from scenes or context

        public void LoadScene(string sceneName, DiContainer[] parentContainer = null)
        {
            LoadScene(sceneName, null, null, parentContainer);
        }

        public void LoadScene(string sceneName, string[] decoratorScenes, string[] additiveScenes, DiContainer[] parentContainers = null)
        {
            Debug.Log($"Load scene: {sceneName} with decorator: {decoratorScenes} and additive {additiveScenes }");

            if (_route is { IsShow: false })
            {
                _route.RouteShow();
            }
            else
            {
                _route = _uiScene.RouteTo(ViewLoadingKey);
            }
          
            MainThreadDispatcher.StartCoroutine(LoadingSceneProcess(sceneName, decoratorScenes, additiveScenes, parentContainers));
        }

        public IDisposable Report(ISceneLoaderListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            LinkedListNode<ISceneLoaderListener> node = _listenerList.AddLast(listener);
            return new ListenerBind(node);
        }

        #endregion

        #region ISceneLoaderController Implementation

        void ISceneLoaderController.SetCurrentSceneController(SceneControllerBase sceneController)
        {
            Debug.Log("Set current scene controller: " + sceneController);

            _currentSceneController = sceneController;

            if (!InLoading) //TODO: First start scene must be initialized without loading
            {
                MainThreadDispatcher.StartCoroutine(InitializeFirstScene());
            }
        }

        #endregion

        private IEnumerator DisposeCurrentScene()
        {
            Assert.IsNotNull(_currentSceneController);

            if (SceneUnLoadCallback != null)
            {
                SceneUnLoadCallback.Invoke(this, CurrentScene);
                SceneUnLoadCallback = null;
            }
            
            SceneUnload?.Invoke(this, CurrentScene);
            
            yield return new WaitForSeconds(_config.Content.DelayStartLoading);

            foreach (IScenePreUnloadEvent scenePostLoadEvent in _currentSceneController
                                                                .SceneContainer.ResolveAll<IScenePreUnloadEvent>())
            {
                scenePostLoadEvent.OnScenePreUnload();
            }

            Debug.Log("Dispose current scene");
            _routineDispatcher.EnqueueRoutine(((ISceneControllerLoader) _currentSceneController).Unload());

            while (_routineDispatcher.HaveActiveRoutines)
            {
                _routineDispatcher.Do();
                yield return null;
            }

            _currentSceneController = null;
        }

        private IEnumerator UnloadAllSubScene()
        {
            foreach (Scene scene in _additiveScenes.Concat(_decoratorScenes))
            {
                AsyncOperation unloadSceneOperation = SceneManager.UnloadSceneAsync(scene);
                while (!unloadSceneOperation.isDone)
                {
                    yield return null;
                }
            }

            _additiveScenes.Clear();
            _decoratorScenes.Clear();
        }

        private IEnumerator InitializeFirstScene()
        {
            InLoading = true;
            _currentScene = SceneManager.GetActiveScene();
            yield return MainThreadDispatcher.StartCoroutine(InitializeCurrentScene());
            InLoading = false;
        }

        private IEnumerator InitializeCurrentScene()
        {
            Assert.IsNotNull(_currentSceneController);
            
            if (ScenePostLoadCallback != null)
            {
                ScenePostLoadCallback.Invoke(this, CurrentScene);
                ScenePostLoadCallback = null;
            }
            
            _routineDispatcher.EnqueueRoutine(((ISceneControllerLoader) _currentSceneController).Load());

            while (_routineDispatcher.HaveActiveRoutines)
            {
                _routineDispatcher.Do();
                yield return null;
            }

            foreach (IScenePostLoadEvent scenePostLoadEvent in _currentSceneController
                                                               .SceneContainer.ResolveAll<IScenePostLoadEvent>())
            {
                scenePostLoadEvent.OnScenePostLoad();
            }

            SceneLoaded?.Invoke(this, CurrentScene);
            
            yield return new WaitForSeconds(_config.Content.DelayEndLoading);

            Debug.Log($"End loading scene: {CurrentScene.name}");
        }

        private IEnumerator LoadingSceneProcess(
            string targetScene,
            IReadOnlyList<string> decoratorScenes,
            IReadOnlyCollection<string> additiveScenes,
            DiContainer[] parentContainers)
        {
            while (InLoading)
            {
                yield return null;
            }

            InLoading = true;

            yield return null;

            yield return MainThreadDispatcher.StartCoroutine(DisposeCurrentScene());
            yield return MainThreadDispatcher.StartCoroutine(UnloadAllSubScene());

            SceneContext.ParentContainers = parentContainers?.Length == 0 ? null : parentContainers;

            if (decoratorScenes != null && decoratorScenes.Count > 0)
            {
                for (var i = 0; i < decoratorScenes.Count; i++)
                {
                    string sceneName = decoratorScenes[i];
                    AsyncOperation loadSceneOperation =
                        SceneManager.LoadSceneAsync(sceneName, i == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive);

                    while (!loadSceneOperation.isDone)
                    {
                        yield return null;
                    }

                    _decoratorScenes.Add(SceneManager.GetSceneByName(sceneName));
                }

                OnStartLoadingScene(targetScene);

                AsyncOperation mainSceneOperation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);

                while (!mainSceneOperation.isDone)
                {
                    OnLoadingSceneProgress(targetScene, mainSceneOperation.progress);
                    yield return null;
                }

                _currentScene = SceneManager.GetSceneByName(targetScene);
                SceneManager.SetActiveScene(_currentScene);
            }
            else
            {
                OnStartLoadingScene(targetScene);

                AsyncOperation mainSceneOperation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
                while (!mainSceneOperation.isDone)
                {
                    OnLoadingSceneProgress(targetScene, mainSceneOperation.progress);
                    yield return null;
                }

                _currentScene = SceneManager.GetActiveScene();
            }

            if (additiveScenes != null && additiveScenes.Count > 0)
            {
                foreach (string sceneName in additiveScenes)
                {
                    AsyncOperation loadAdditiveSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                    while (!loadAdditiveSceneOperation.isDone)
                    {
                        yield return null;
                    }

                    _additiveScenes.Add(SceneManager.GetSceneByName(sceneName));
                }
            }

            foreach (GameObject rootGameObject in _currentScene.GetRootGameObjects())
            {
                var sceneContext = rootGameObject.GetComponentInChildren<SceneContext>();
                if (sceneContext && !sceneContext.Initialized)
                {
                    sceneContext.Run();
                }
            }

            while (_currentSceneController == null)
            {
                yield return null;
            }

            OnEndLoadingScene(targetScene);

            yield return MainThreadDispatcher.StartCoroutine(InitializeCurrentScene());

            InLoading = false;
            _route?.RouteHide();
        }

        private void OnStartLoadingScene(string sceneName)
        {
            Debug.Log($"Start loading scene: {sceneName}");

            foreach (ISceneLoaderListener sceneLoaderListener in _listenerList)
            {
                sceneLoaderListener.StartSceneLoading(sceneName);
                sceneLoaderListener.SceneLoadingProgress(sceneName, 0);
            }
        }

        private void OnEndLoadingScene(string sceneName)
        {
            Debug.Log($"Start initialization scene: {sceneName}");

            foreach (ISceneLoaderListener sceneLoaderListener in _listenerList)
            {
                sceneLoaderListener.SceneLoadingProgress(sceneName, 1);
                sceneLoaderListener.StartSceneLoading(sceneName);
            }
        }

        private void OnLoadingSceneProgress(string sceneName, float progress)
        {
            foreach (ISceneLoaderListener sceneLoaderListener in _listenerList)
            {
                sceneLoaderListener.SceneLoadingProgress(sceneName, progress);
            }
        }

        private struct ListenerBind : IDisposable
        {
            private readonly LinkedListNode<ISceneLoaderListener> _node;

            public ListenerBind(LinkedListNode<ISceneLoaderListener> node)
            {
                _node = node;
            }

            #region IDisposable Implementation

            public void Dispose()
            {
                _node.List?.Remove(_node);
            }

            #endregion
        }

        public void Initialize()
        {
            _uiScene = _uiManager.CreateUiScene("loading_scene");
            
            _uiScene.AddRoute<LoadingPresenter>(ViewLoadingKey);
        }
    }
}