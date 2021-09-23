using GameLayer.WeaponSystem;

namespace GameLayer.CharacterSystem.CharacterWeaponsSystem.ShootingSystem
{
    public interface ICharacterWeaponShootingControl
    {
        void Init(IWeaponComponent component);
        void Update();
    }
}