using System.Collections.Generic;
using CommonLayer.DataContext;
using CommonLayer.SceneControllers;
using CommonLayer.SceneControllers.Routines;
using CommonLayer.UserInterface.Managers;
using CommonLayer.UserInterface.RouteSystem.Scenes;
using CommonLayer.UserInterface.RouteSystem.Scenes.Nodes;
using MenuLayer.UserInterface;

namespace MenuLayer
{
    public sealed class MenuSceneController : SceneControllerBase
    {
        private readonly IUiManager _uiManager;
        private readonly RootDataContext _rootCtx;
        private const string MenuSceneKey = "ui_menu_scene";
        private const string MenuHudKey = "menu_hud_presenter";
        private const string MenuSettingsKey = "menu_settings_presenter";
        
        private IUiSceneController _uiScene;

        public MenuSceneController(IUiManager uiManager,RootDataContext rootCtx)
        {
            _uiManager = uiManager;
            _rootCtx = rootCtx;
        }
        
        protected override IEnumerator<TaskStatus> OnLoading()
        {
         
            if (!_rootCtx.IsInitialized)
            {
                _rootCtx.Load();
            }
         
            while (!_rootCtx.IsInitialized)
            {
                yield return TaskStatus.Continue;
            }

            
            BindUiScene();
            var route = _uiScene.RouteTo(MenuHudKey);
            
           
            while (!route.IsShow)
            {
                yield return TaskStatus.Continue;
            }
        }
        
        private void BindUiScene()
        {
            _uiScene = _uiManager.CreateUiScene(MenuSceneKey);
            _uiScene.AddRoute<MenuHudPresenter>(MenuHudKey, gameHud =>
            {
                gameHud.ToSub<MenuSettingsPresenter>(MenuSettingsKey);
            });
        }
        
        protected override IEnumerator<TaskStatus> OnUnloading()
        {
            if (!IsLoaded)
            {
                yield break;
            }
            
            var route = _uiScene.PeekRoute();
            route?.RouteHide();
            _uiScene.ForceEnd();
        }
        
        
    }
}