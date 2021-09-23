using System;
using CommonLayer.ResourceSystem.Config.Interfaces;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Config.Impl
{
    [Serializable]
    public sealed class BulletConfig : IBulletConfig
    {
        [SerializeField] private BulletType type;

        [SerializeField] private float damage;

        [SerializeField] private float force;

        [SerializeField] private float explosionRadius;

        public float Damage => damage;

        public float Force => force;

        public float ExplosionRadius => explosionRadius;

        public BulletType Type => type;

        public int GetHash()
        {
            return damage.GetHashCode() + force.GetHashCode() + explosionRadius.GetHashCode() + type.GetHashCode();
        }
    }
}