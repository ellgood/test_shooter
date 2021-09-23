using System.Collections.Generic;

namespace GameLayer.SpawnSystem
{
    public interface ISpawnPointsPreset
    {
        IReadOnlyList<SpawnPointInfo> SpawnPoints { get; }
    }
}