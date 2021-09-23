using UnityEngine;

namespace GameLayer.WeaponSystem
{
    public interface IWeaponComponent
    {
        Transform Slot { get; }

        Transform Body { get; }

        Transform Muzzle { get; }

        Transform Aim { get; }
    }
}