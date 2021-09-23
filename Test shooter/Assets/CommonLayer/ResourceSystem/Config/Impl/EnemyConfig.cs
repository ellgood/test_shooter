using System;
using CommonLayer.ResourceSystem.Config.Data;
using CommonLayer.ResourceSystem.Config.Interfaces;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Config.Impl
{
    [Serializable]
    public sealed class EnemyConfig : ConfigBase, IEnemyConfig
    {
        [SerializeField]
        private IntRestraintParam health;

        public IntRestraintParam Health => health;
        public override int GetHash()
        {
            return health.GetHashCode();
        }
    }
}