using GameLayer.WeaponSystem;

namespace GameLayer.CharacterSystem.CharacterWeaponsSystem.ShootingSystem
{
    public interface IShootingSetWeaponProvider
    {
        void SetWeapon(IWeapon weapon);
    }
}