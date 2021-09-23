using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace GameLayer.WeaponSystem
{
    public sealed class Weapon : IWeapon
    {
        public GameObject Instance { get; }
        public WeaponData Data { get; }
       

        public Weapon(GameObject instance, WeaponData data)
        {
            Data = data;
            Instance = instance;
        }
    }
}