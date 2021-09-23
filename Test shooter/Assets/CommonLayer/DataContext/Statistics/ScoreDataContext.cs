using System;
using System.Collections;
using CommonLayer.SaveLoadSystem;


namespace CommonLayer.DataContext.Statistics
{
    public sealed class ScoreDataContext : DataContextBase,IScoreDataContext
    {
        private const string SaveKey = "Score";
        private readonly ISaveLoadManager _saveLoadManager;

        public ScoreDataContext(ISaveLoadManager saveLoadManager)
        {
            _saveLoadManager = saveLoadManager;
        }

        private ScoreData _scoreData;
        protected override IEnumerator OnInitializeProcess()
        {
            LoadScore();
            yield break;
        }
        protected override void OnReset()
        {
            SaveScore();
        }
        private void SaveScore()
        {
            _saveLoadManager.Save(SaveKey,_scoreData);
        }

        private void LoadScore()
        {
            _scoreData = _saveLoadManager.TryLoad(SaveKey, out ScoreData data) ? data : new ScoreData();
        }

        public event Action ScoreChanged;
        public int Score => _scoreData.Score;
        
        public void AddScore(int score)
        {
            _scoreData.AddScore(score);
            ScoreChanged?.Invoke();
        }

        public void ResetScore()
        {
            _scoreData.ResetScore();
            ScoreChanged?.Invoke();
        }
    }
}