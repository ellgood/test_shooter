using System;

namespace CommonLayer.DataContext.Statistics
{
    [Serializable]
    public sealed class ScoreData
    {
        public ScoreData()
        {
            Score = 0; 
        }
        public int Score { get; private set; }

        public void AddScore(int score)
        {
            Score += score;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}