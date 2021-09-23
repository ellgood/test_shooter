using System;
using CommonLayer.SceneControllers;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Routes;
using UnityEngine;

namespace MenuLayer.UserInterface
{
    public sealed class MenuHudPresenter : PresenterBase
    {
        private readonly ISceneLoader _sceneLoader;
        private IRoute _settingsRoute;
        private const string MenuSettingsKey = "menu_settings_presenter";
        public override string ViewKey => "menu_hud_view";

        public MenuHudPresenter(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        protected override void OnInit()
        {
            BindContext.Signal("play_button", OnPlayButton);
            BindContext.Signal("settings_button", OnSettingsButton);
            BindContext.Signal("quit_button", OnQuitButton);
        }

        protected override void OnShow(Action onShowed)
        {
            base.OnShow(onShowed);
            _settingsRoute ??= Route.RouteTo(MenuSettingsKey,false);
        }
        
        
        private void OnQuitButton()
        {
            Application.Quit();
        }

        private void OnSettingsButton()
        {
            _settingsRoute.RouteShow();
        }

        private void OnPlayButton()
        {
            _sceneLoader.LoadScene("Game");
        }
    }
}