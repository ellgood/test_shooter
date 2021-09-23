using System;
using Zenject;

namespace GameLayer.SpawnSystem
{
    public sealed class SpawnManager : ISpawnManager, IInitializable
    {
        private readonly ISpawnPointsPreset _spawnPointsPreset;

        private int _count;

        private SpawnPointInfo[] _points;
        private bool[] _pointStates;

        public SpawnManager(ISpawnPointsPreset spawnPointsPreset)
        {
            _spawnPointsPreset = spawnPointsPreset;
        }

        #region IInitializable Implementation

        public void Initialize()
        {
            _points = Array.Empty<SpawnPointInfo>();
            _pointStates = Array.Empty<bool>();

            LoadSpawnPreset(_spawnPointsPreset);
        }

        #endregion

        public bool TryGetFreeSpawnPoint(SpawnPointFlags pointFlags, out SpawnPointInfo point)
        {
            for (var index = 0; index < _count; index++)
            {
                if (_points[index].PointFlags != pointFlags || !_pointStates[index]) continue;

                _pointStates[index] = false;
                point = _points[index];
                return true;
            }

            point = default;
            return false;
        }

        #region ISpawnManager Implementation

        private void LoadSpawnPreset(ISpawnPointsPreset preset)
        {
            _count = preset.SpawnPoints.Count;

            Array.Resize(ref _points, _count);
            Array.Resize(ref _pointStates, _count);

            for (var index = 0; index < _count; index++)
            {
                var data = preset.SpawnPoints[index];
                _points[index] = data;
                _pointStates[index] = true;
            }
        }

        #endregion
    }
}