using System;
using UnityEngine;

namespace GameLayer.SpawnSystem
{
    [Serializable]
    public struct SpawnPointInfo : IEquatable<SpawnPointInfo>
    {
        [SerializeField] private SpawnPointFlags pointFlags;

        [SerializeField] private Vector3 position;

        [SerializeField] private float rotation;

        public SpawnPointInfo(Vector3 position, float rotation = 0, SpawnPointFlags pointFlags = SpawnPointFlags.None)
        {
            this.position = position;
            this.rotation = rotation;
            this.pointFlags = pointFlags;
        }

        public Vector3 Position => position;
        public float Rotation => rotation;
        public SpawnPointFlags PointFlags => pointFlags;

        public bool Equals(SpawnPointInfo other)
        {
            return
                position == other.position &&
                Mathf.Approximately(rotation, other.rotation) &&
                pointFlags == other.pointFlags;
        }
    }
}