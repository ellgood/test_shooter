using System.Collections.Generic;
using CommonLayer.SceneControllers.Routines;
using CommonLayer.UserInterface.Managers;
using UnityEngine;
using Zenject;

namespace CommonLayer.SceneControllers
{
    public abstract class SceneControllerBase : IInitializable, ISceneControllerLoader, ISceneController
    {
        [Inject]
        internal ISceneLoaderController SceneManager { get; private set; }

        public ISceneLoader SceneLoader => SceneManager;

        #region IInitializable Implementation

        public void Initialize()
        {
            SceneManager.SetCurrentSceneController(this);
        }

        #endregion

        #region ISceneController Implementation

        [Inject]
        public IUiManager UiManager { get; private set; }

        [Inject]
        public DiContainer SceneContainer { get; private set; }

        public bool IsLoaded { get; private set; }

        #endregion

        #region ISceneControllerLoader Implementation

        IEnumerator<TaskStatus> ISceneControllerLoader.Load()
        {
            using (IEnumerator<TaskStatus> onLoad = OnLoading())
            {
                while (onLoad.MoveNext())
                {
                    if (onLoad.Current.Equals(TaskStatus.TaskFailed))
                    {
                        yield break;
                    }

                    yield return onLoad.Current;
                }
            }

            IsLoaded = true;

            foreach (ICurrentSceneLoadedEvent currentSceneLoadedEvent in SceneContainer.ResolveAll<ICurrentSceneLoadedEvent>())
            {
                currentSceneLoadedEvent.SceneLoaded();
            }

            OnLoaded();

            Debug.Log($"Load {GetType()} successful");
        }

        IEnumerator<TaskStatus> ISceneControllerLoader.Unload()
        {
            Debug.Log($"Unload {GetType()} after {IsLoaded} loaded");

            if (IsLoaded)
            {
                foreach (ICurrentSceneUnloadEvent currentSceneUnloadEvent in SceneContainer.ResolveAll<ICurrentSceneUnloadEvent>())
                {
                    currentSceneUnloadEvent.SceneUnload();
                }

                OnUnload();
            }

            return OnUnloading();
        }

        #endregion

        protected virtual void OnLoaded() { }

        protected virtual void OnUnload() { }

        protected virtual IEnumerator<TaskStatus> OnLoading()
        {
            yield break;
        }

        protected virtual IEnumerator<TaskStatus> OnUnloading()
        {
            yield break;
        }
    }
}