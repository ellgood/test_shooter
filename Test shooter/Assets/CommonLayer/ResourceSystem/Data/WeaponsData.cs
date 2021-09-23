using System;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Data
{
    [Serializable]
    public sealed class WeaponsData
    {
        [SerializeField] 
        private WeaponData pistolData;
        
        [SerializeField] 
        private WeaponData machineGunData;
        
        [SerializeField] 
        private WeaponData rocketLauncherData;

        public WeaponData PistolData => pistolData;

        public WeaponData MachineGunData => machineGunData;

        public WeaponData RocketLauncherData => rocketLauncherData;
    }
}