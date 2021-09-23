using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;
using Zenject;

namespace GameLayer.WeaponSystem
{
    public sealed class WeaponFactory : PlaceholderFactory<Vector3,Transform,WeaponData,IWeapon>
    {
        
    }
}