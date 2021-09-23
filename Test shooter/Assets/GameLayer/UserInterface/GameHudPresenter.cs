using System;
using CommonLayer.DataContext.Statistics;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.DataBinding.Binds;
using CommonLayer.UserInterface.Presenter;
using CommonLayer.UserInterface.RouteSystem.Routes;
using UniRx;
using UnityEngine;

namespace GameLayer.UserInterface
{
    public class GameHudPresenter : PresenterBase
    {
        private const string GameSettingsKey = "game_settings_presenter";
        
        private readonly ReactiveProperty<string> _scoreProp = new ReactiveProperty<string>();
        private readonly ReactiveProperty<bool> _aimActivationProp = new ReactiveProperty<bool>();
        private readonly IScoreDataContext _scoreCtx;
        
      
        private IRoute _settingsRoute;

        public GameHudPresenter(IScoreDataContext scoreCtx)
        {
            _scoreCtx = scoreCtx;
        }
        
        public override string ViewKey => "game_hud_view";

        protected override void OnInit()
        {
            BindContext.TextData("score", _scoreProp);
            BindContext.Bind("aim_activation", _aimActivationProp, RelationType.OneWay);
        }

        protected override void OnShow(Action onShowed)
        {
            base.OnShow(onShowed);

            _settingsRoute ??= Route.RouteTo(GameSettingsKey, false);
      
            _aimActivationProp.Value = true;
            OnScoreChanged();
            _scoreCtx.ScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged()
        {
            _scoreProp.Value = $"Score {_scoreCtx.Score}";
        }

        protected override void OnHide(Action onHidden)
        {
            base.OnHide(onHidden);
            _scoreCtx.ScoreChanged -= OnScoreChanged;
        }
    }
}