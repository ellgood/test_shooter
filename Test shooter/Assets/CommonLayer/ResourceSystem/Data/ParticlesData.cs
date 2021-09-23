using System;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Data
{
    [Serializable]
    public sealed class ParticlesData
    {
        [SerializeField]
        private GameObject muzzleFlash;
        [SerializeField]
        private GameObject simpleHit;
        [SerializeField]
        private GameObject explodeHit;

        [SerializeField] 
        private float poolLifeTime;
        
        [SerializeField] 
        private int minPoolCount;

        public GameObject MuzzleFlash => muzzleFlash;

        public GameObject SimpleHit => simpleHit;

        public GameObject ExplodeHit => explodeHit;

        public float PoolLifeTime => poolLifeTime;

        public int MinPoolCount => minPoolCount;
    }
}