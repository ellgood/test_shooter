using System;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Managers;
using CommonLayer.UserInterface.Objects;
using CommonLayer.UserInterface.RouteSystem.Routes;
using CommonLayer.UserInterface.Views;
using UnityEngine.Profiling;

namespace CommonLayer.UserInterface.Presenter
{
    public abstract class PresenterBase : DisposableObject, IPresenterBridge
    {
        private ViewBase _view;

        protected PresenterBase()
        {
            BindContext = BindContext.Pool.Rent();
        }

        public IRoute Route { get; private set; }

        protected BindContext BindContext { get; }

        #region IPresenter Implementation

        public abstract string ViewKey { get; }

        #endregion

        #region IPresenterBridge Implementation

        ViewBase IPresenterBridge.View
        {
            get => _view;
            set => _view = value;
        }

        IMainUiManager IPresenterBridge.UiManager { get; set; }

        #endregion

        #region IRoutedPresenter Implementation

        IRoute IRoutedPresenter.Route
        {
            set => Route = value;
        }

        void IRoutedPresenter.RouteInitialize()
        {
            OnInit();
            Profiler.BeginSample($"{GetType().Name}.SetContext");
            _view.SetContext(BindContext);
            Profiler.EndSample();
        }

        void IRoutedPresenter.RouteClose()
        {
            Dispose();
        }

        void IRoutedPresenter.RouteHide(Action onHidden)
        {
            ThrowIfDisposedDebugOnly();

            OnHide(() =>
            {
                _view.Hide(() =>
                {
                    onHidden?.Invoke();
                });
            });
        }

        void IRoutedPresenter.RouteShow(Action onShowed)
        {
            ThrowIfDisposedDebugOnly();

            OnShow(() =>
            {
                if (!_view.gameObject.activeSelf)
                {
                    _view.gameObject.SetActive(true);
                }

                _view.Show(onShowed);
            });
        }

        void IRoutedPresenter.RouteShowed()
        {
            OnShowed();
        }

        void IRoutedPresenter.RouteHidden()
        {
            _view.gameObject.SetActive(false);

            OnHidden();
        }

        #endregion

        protected sealed override void OnDispose()
        {
            OnClose();

            if (_view)
            {
                _view.DropContext();
                ((IPresenterBridge) this).UiManager.ReturnView(_view);
            }
            else
            {
                _view = null;
            }

            BindContext.Pool.Return(BindContext);
        }

        protected virtual void OnShowed() { }

        protected virtual void OnHidden() { }

        protected virtual void OnInit() { }

        protected virtual void OnShow(Action onShowed)
        {
            onShowed.Invoke();
        }

        protected virtual void OnHide(Action onHidden)
        {
            onHidden.Invoke();
        }

        protected virtual void OnClose() { }
    }
}