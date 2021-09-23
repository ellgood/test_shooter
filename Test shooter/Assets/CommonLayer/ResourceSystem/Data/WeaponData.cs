using System;
using CommonLayer.ResourceSystem.Config.Impl;
using CommonLayer.ResourceSystem.Config.Interfaces;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Data
{
    [Serializable]
    public sealed class WeaponData : GameObjectDataBase<WeaponConfig,IWeaponConfig>
    {
        [SerializeField] 
        private WeaponType weaponType;
        [SerializeField] 
        private Vector3 slotPosition;
        
        [SerializeField] 
        private Vector3 muzzlePosition;
        public Vector3 SlotPosition => slotPosition;
        public Vector3 MuzzlePosition => muzzlePosition;
        public WeaponType WeaponType => weaponType;
    }
}