using System;
using System.Collections.Generic;
using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Interface;
using CommonLayer.UserInterface.Config;
using CommonLayer.UserInterface.Layering;
using CommonLayer.UserInterface.Pooling;
using CommonLayer.UserInterface.Pooling.AutoTrimming;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.Views;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Object = UnityEngine.Object;

namespace CommonLayer.UserInterface.Managers
{
    public sealed class MainUiManager : IMainUiManager
    {
        private static LayerManager _layerManager;

        private readonly Dictionary<string, UnityComponentPool<ViewBase>> _viewsPool =
            new Dictionary<string, UnityComponentPool<ViewBase>>();

        private readonly Dictionary<ViewBase, UnityComponentPool<ViewBase>> _rentedViews =
            new Dictionary<ViewBase, UnityComponentPool<ViewBase>>();

        private readonly UiConfiguration _config;

        public MainUiManager(IResourcesController resourcesController)
        {
            _config = resourcesController.GetData<UiConfiguration>();

            if (_layerManager == null)
            {
                _layerManager = Object.Instantiate(_config.LayerManager);
                Object.DontDestroyOnLoad(_layerManager.gameObject);
            }
        }

        #region IMainUiManager Implementation

        public Rect CanvasRect => _layerManager.CanvasRect;

        public T Create<T>(DiContainer container)
            where T : PresenterBase
        {
            return Create<T>(container, null);
        }

        public T Create<T>(DiContainer container, string viewKey)
            where T : PresenterBase
        {
            return Create<T>(container, viewKey, null);
        }

        public T Create<T>(DiContainer container, string viewKey, params object[] args)
            where T : PresenterBase
        {
            T presenter = args != null ? container.Instantiate<T>(args) : container.Instantiate<T>();
            ((IPresenterBridge) presenter).UiManager = this;

            SetView(viewKey, presenter, container);
            return presenter;
        }

        public ViewBase RentView(string viewKey)
        {
            UnityComponentPool<ViewBase> pool = GetViewPool(viewKey);
            if (pool == null)
            {
                throw new ArgumentException($"Pool {viewKey} not found", nameof(viewKey));
            }

            ViewBase rentingView = pool.Rent();
            _rentedViews.Add(rentingView, pool);
            return rentingView;
        }

        public void ReturnView(ViewBase view)
        {
            if (_rentedViews.TryGetValue(view, out UnityComponentPool<ViewBase> pool))
            {
                _rentedViews.Remove(view);

                view.DropContext();
                view.SetDefault();

                pool.Return(view);
            }
            else
            {
                throw new InvalidOperationException("This object doesn't rented in this ui manager.");
            }
        }

        #endregion

        private void SetView<T>(string viewKey, T presenter, DiContainer container)
            where T : IPresenterBridge
        {
            string key = string.IsNullOrWhiteSpace(viewKey) ? presenter.ViewKey : viewKey;

            ViewBase view = RentView(key);
            container.InjectGameObject(view.gameObject);

            presenter.View = view;
        }

        private UnityComponentPool<ViewBase> GetViewPool(string viewKey)
        {
            if (!_viewsPool.TryGetValue(viewKey, out UnityComponentPool<ViewBase> pool))
            {
                if (_config.Content.TryGetValue(viewKey, out ViewInfo viewInfo))
                {
                    ViewBase originalView = viewInfo.View;
                    UiLayer layer = _layerManager.GetLayer(viewInfo.Layer);

                    Assert.IsNotNull(originalView, $"originalView(\"{viewKey}\") != null");

                    pool = new UnityComponentPool<ViewBase>(originalView, viewInfo.UseSafeZone ? layer.SafeZoneContainer : layer.Container)
                    {
                        AutoTrimOption = new AutoTrimOption
                        {
                            Enabled = true,
                            TrimDelay = viewInfo.PoolLifeTime,
                            MinSize = viewInfo.MaxDefaultPoolCount
                        }
                    };
                    _viewsPool[viewKey] = pool;
                }
                else
                {
                    throw new InvalidOperationException($"Not found view {viewKey}");
                }
            }

            return pool;
        }
    }
}