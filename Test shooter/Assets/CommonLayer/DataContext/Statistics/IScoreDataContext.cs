using System;

namespace CommonLayer.DataContext.Statistics
{
    public interface IScoreDataContext : IDataContext
    {
        event Action ScoreChanged;
        int Score { get; }
        void AddScore(int score);
        void ResetScore();
    }
}