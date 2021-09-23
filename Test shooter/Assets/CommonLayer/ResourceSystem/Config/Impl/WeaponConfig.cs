using System;
using CommonLayer.ResourceSystem.Config.Interfaces;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Config.Impl
{
    [Serializable]
    public class WeaponConfig : ConfigBase, IWeaponConfig
    {
        [SerializeField] 
        private float range;
        [SerializeField] 
        private float fireRate;
        [SerializeField] 
        private float damageMtp;
        public float Range => range;
        public float FireRate => fireRate;

        public float DamageMtp => damageMtp;

        public override int GetHash()
        {
            return range.GetHashCode() + 
                   fireRate.GetHashCode() + 
                   damageMtp.GetHashCode();
        }
    }
}