using System.Collections.Generic;
using UnityEngine;

namespace GameLayer.SpawnSystem
{
    public sealed class SpawnPointsPreset : MonoBehaviour, ISpawnPointsPreset
    {
        [SerializeField]
        private SpawnPointInfo[] spawnPoints;

        #region ISpawnPointsPreset Implementation

        public IReadOnlyList<SpawnPointInfo> SpawnPoints => GetWorldSpawnPoint();

        #endregion

        private SpawnPointInfo[] GetWorldSpawnPoint()
        {
            var converted = new SpawnPointInfo[spawnPoints.Length];
            for (var i = 0; i < converted.Length; i++)
            {
                converted[i] = ConvertLocalToGlobal(spawnPoints[i]);
            }

            return converted;
        }
        
        private SpawnPointInfo ConvertLocalToGlobal(SpawnPointInfo local)
        {
            Transform t = transform;
            Vector3 worldPos = t.localToWorldMatrix.MultiplyPoint3x4(local.Position);
            return new SpawnPointInfo(worldPos, t.rotation.eulerAngles.y + local.Rotation, local.PointFlags);
        }
    }
}