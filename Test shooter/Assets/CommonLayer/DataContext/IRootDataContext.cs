using CommonLayer.DataContext.Settings;
using CommonLayer.DataContext.Statistics;

namespace CommonLayer.DataContext
{
    public interface IRootDataContext
    {
        ICharacterSettingsDataContext CharacterSettingsCtx { get; }
        IScoreDataContext ScoreCtx { get; }
    }
}