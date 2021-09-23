using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;

namespace CommonLayer.DataContext
{
    public abstract class DataContextBase : IDataContext
    {
        private readonly IDataContext[] _child;

        private bool _isInitialized;
        private IDisposable _disposable;

        protected DataContextBase() : this(Array.Empty<IDataContext>()) { }

        protected DataContextBase(params IDataContext[] childContext)
        {
            _child = childContext;
        }

        public bool IsCancelled { get; private set; }
        

        #region IDataContext Implementation

        public event Action<StatusStep> ContextStatusChanged;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized && _child.All(c => c.IsInitialized);
            }
            private set => _isInitialized = value;
        }

        public virtual bool IsLoaded => IsInitialized;

        public bool IsInitializing { get; private set; }

        public void Load()
        {
            if (IsInitializing || IsInitialized)
            {
                return;
            }

            _disposable = Observable.FromCoroutine(InitializeCoroutine).Subscribe();
        }

        public void Reset()
        {
            if (IsInitializing || _isInitialized)
            {
                _disposable?.Dispose();
                IsInitializing = false;
                _isInitialized = false;
            }

            OnReset();
        }

        #endregion

        protected virtual void OnReset() { }

        protected virtual void OnBeforeInitializeProcess()
        {
            ChangeLoadingStatus(StatusStep.Start);
            Debug.Log($"Initialize data context [{ GetType().Name}]");
        }

        protected virtual void OnAfterInitializeProcess()
        {
            ChangeLoadingStatus(StatusStep.Complete);
            Debug.Log($"Initialize finish data context [{GetType().Name}]");
        }

        protected abstract IEnumerator OnInitializeProcess();

        private void ChangeLoadingStatus(StatusStep status)
        {
            ContextStatusChanged?.Invoke(status);
        }

        private IEnumerator InitializeCoroutine()
        {
            OnBeforeInitializeProcess();
            IsInitializing = true;
            try
            {
                foreach (IDataContext dataContext in _child)
                {
                    if (dataContext.IsInitialized)
                    {
                        continue;
                    }

                    dataContext.ContextStatusChanged += OnSubContextStatusChanged;
                    dataContext.Load();

                    while (!dataContext.IsInitialized)
                    {
                        if (IsCancelled)
                        {
                            dataContext.Reset();
                            yield break;
                        }

                        yield return null;
                    }

                    dataContext.ContextStatusChanged -= OnSubContextStatusChanged;
                }

                IEnumerator enumerator = OnInitializeProcess();
                ChangeLoadingStatus(StatusStep.Process);
                while (enumerator.MoveNext())
                {
                    if (IsCancelled)
                    {
                        yield break;
                    }

                    yield return enumerator.Current;
                }

                OnAfterInitializeProcess();
                IsInitialized = true;
            }
            finally
            {
                IsInitializing = false;
            }
        }

        private void OnSubContextStatusChanged(StatusStep status)
        {
            ContextStatusChanged?.Invoke(status);
        }
    }
}