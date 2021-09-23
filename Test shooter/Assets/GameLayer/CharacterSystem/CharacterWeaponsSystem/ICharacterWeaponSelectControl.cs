using GameLayer.WeaponSystem;

namespace GameLayer.CharacterSystem.CharacterWeaponsSystem
{
    public interface ICharacterWeaponSelectControl
    {
        void Init(IWeaponComponent component);
        void Update();
    }
}