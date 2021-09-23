using System.Collections.Generic;
using CommonLayer.DataContext;
using CommonLayer.SceneControllers;
using CommonLayer.SceneControllers.Routines;
using CommonLayer.UserInterface.Managers;
using CommonLayer.UserInterface.RouteSystem.Scenes;
using CommonLayer.UserInterface.RouteSystem.Scenes.Nodes;
using GameLayer.UserInterface;

namespace GameLayer
{
    public class GameSceneController : SceneControllerBase
    {
        private const string GameSceneKey = "ui_game_scene";
        private const string GameHudKey = "game_hud_presenter";
        private const string GameSettingsKey = "game_settings_presenter";

        private readonly RootDataContext _rootCtx;

        private readonly IUiManager _uiManager;

        private IUiSceneController _uiScene;

        public GameSceneController(IUiManager uiManager, RootDataContext rootCtx)
        {
            _uiManager = uiManager;
            _rootCtx = rootCtx;
        }

        protected override IEnumerator<TaskStatus> OnLoading()
        {
            if (!_rootCtx.IsInitialized) _rootCtx.Load();

            while (!_rootCtx.IsInitialized) yield return TaskStatus.Continue;


            BindUiScene();
            var route = _uiScene.RouteTo(GameHudKey);

            while (!route.IsShow) yield return TaskStatus.Continue;
        }

        private void BindUiScene()
        {
            _uiScene = _uiManager.CreateUiScene(GameSceneKey);
            _uiScene.AddRoute<GameHudPresenter>(GameHudKey,
                gameHud => { gameHud.ToSub<GameSettingsPresenter>(GameSettingsKey); });
        }

        protected override IEnumerator<TaskStatus> OnUnloading()
        {
            if (!IsLoaded) yield break;

            var route = _uiScene.PeekRoute();
            route?.RouteHide();
            _uiScene.ForceEnd();
        }
    }
}