using System;
using CommonLayer.DataContext.Settings;
using CommonLayer.SceneControllers;
using CommonLayer.UserInterface.Presenter.Settings;
using CommonLayer.UserInterface.RouteSystem.Routes;
using GameLayer.CharacterSystem;
using UniRx;
using UnityEngine;


namespace GameLayer.UserInterface
{
    public sealed class GameSettingsPresenter : SettingsPresenterBase
    {
        private readonly IGameCharacterController _gameCharacterController;
        private readonly ISceneLoader _sceneLoader;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public GameSettingsPresenter(
            ICharacterSettingsDataContext characterSettingsCtx, 
            IGameCharacterController gameCharacterController,
            ISceneLoader sceneLoader) : base(characterSettingsCtx)
        {
            _gameCharacterController = gameCharacterController;
            _sceneLoader = sceneLoader;
        }

        public override string ViewKey => "game_settings_view";

        protected override void OnInit()
        {
            base.OnInit();
            MainThreadDispatcher.UpdateAsObservable().Subscribe(OnUpdate).AddTo(_disposable);
        }

        protected override void OnMenuButton()
        {
            Route.RouteBack();
            _sceneLoader.LoadScene("Menu");
        }

        protected override void OnShow(Action onShowed)
        {
            base.OnShow(onShowed);
            _gameCharacterController.SetActive(false);
        }
        
        protected override void OnHide(Action onHidden)
        {
            base.OnHide(onHidden);
            _gameCharacterController.SetActive(true);
        }

        protected override void OnClose()
        {
            base.OnClose();
            _disposable.Clear();
        }

        private void OnUpdate(Unit unit)
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (Route.IsShow)
            {
                Route.RouteHide();
            }
            else
            {
                Route.RouteShow();
            }
        }
    }
}