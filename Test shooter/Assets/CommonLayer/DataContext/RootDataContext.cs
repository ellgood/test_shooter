using System.Collections;
using CommonLayer.DataContext.Settings;
using CommonLayer.DataContext.Statistics;

namespace CommonLayer.DataContext
{
    public sealed class RootDataContext : DataContextBase, IRootDataContext
    {
        public RootDataContext(
            ICharacterSettingsDataContext characterSettingsCtx,
            IScoreDataContext scoreCtx) : base(characterSettingsCtx, scoreCtx)
        {
            CharacterSettingsCtx = characterSettingsCtx;
            ScoreCtx = scoreCtx;
        }

        public ICharacterSettingsDataContext CharacterSettingsCtx { get; }
        public IScoreDataContext ScoreCtx { get; }

        protected override IEnumerator OnInitializeProcess()
        {
            yield break;
        }

        protected override void OnReset()
        {
            CharacterSettingsCtx.Reset();
            ScoreCtx.Reset();
        }
    }
}