using System.Collections.Generic;

namespace GameLayer.WeaponSystem
{
    public interface IWeaponContent
    {
        IWeapon CurrentWeapon { get; }
        IReadOnlyList<IWeapon> Weapons { get; }
    }
}