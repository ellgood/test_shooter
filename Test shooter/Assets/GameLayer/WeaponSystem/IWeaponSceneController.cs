using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace GameLayer.WeaponSystem
{
    public interface IWeaponSceneController
    {
        int Count { get; }
        IWeapon WeaponAttachTo(WeaponType type, Transform parent, bool isActive = true);
        void WeaponReturn(WeaponType type);
    }
}