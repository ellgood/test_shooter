using System;
using System.Collections.Generic;
using CommonLayer.UserInterface.Objects;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Scenes;
using UnityEngine;
using Zenject;

namespace CommonLayer.UserInterface.Managers
{
    public sealed class UiManager : DisposableObject, IUiManager
    {
        private readonly DiContainer _container;
        private readonly IMainUiManager _mainUiManager;

        private readonly List<IPresenter> _createdPresenters = new List<IPresenter>();

        private readonly Dictionary<string, IUiSceneController> _uiScenes = new Dictionary<string, IUiSceneController>();

        public UiManager(DiContainer container, IMainUiManager mainUiManager)
        {
            _container = container;
            _mainUiManager = mainUiManager;
        }

        #region IUiManager Implementation

        public event Action Cleaning;

        public T Create<T>()
            where T : PresenterBase
        {
            return Create<T>(null);
        }

        public T Create<T>(string view)
            where T : PresenterBase
        {
            return Create<T>(view, null);
        }

        public T Create<T>(string view, params object[] args)
            where T : PresenterBase
        {
            var presenter = _mainUiManager.Create<T>(_container, view, args);
            _createdPresenters.Add(presenter);
            return presenter;
        }

        public IUiSceneController CreateUiScene(string sceneKey)
        {
            if (string.IsNullOrWhiteSpace(sceneKey))
            {
                throw new ArgumentException("Scene key can't be null or empty", nameof(sceneKey));
            }

            if (_uiScenes.ContainsKey(sceneKey))
            {
                throw new ArgumentException($"Scene already exist with key {sceneKey}", nameof(sceneKey));
            }

            var scene = new UiScene(sceneKey, this, OnEndSceneCallback);
            _uiScenes.Add(sceneKey, scene);

            Debug.Log($"Ui scene {sceneKey} removed from manager");

            return scene;
        }

        public bool TryGetScene(string sceneKey, out IUiSceneController uiScene)
        {
            return _uiScenes.TryGetValue(sceneKey, out uiScene);
        }

        #endregion

        protected override void OnDispose()
        {
            Cleaning?.Invoke();
            _uiScenes.Clear();

            foreach (IPresenter p in _createdPresenters)
            {
                if (p is DisposableObject disposable && disposable.IsDisposed)
                {
                    continue;
                }

                p.Dispose();
            }

            _createdPresenters.Clear();
        }

        private void OnEndSceneCallback(UiScene scene)
        {
            if (!_uiScenes.ContainsKey(scene.SceneTag))
            {
                Debug.Log($"Ui scene {scene.SceneTag} can't removed from manager because it wasn't found in the manager.");
                return;
            }

            _uiScenes.Remove(scene.SceneTag);
            Debug.Log($"Ui scene removed {scene.SceneTag} from manager");
        }
    }
}