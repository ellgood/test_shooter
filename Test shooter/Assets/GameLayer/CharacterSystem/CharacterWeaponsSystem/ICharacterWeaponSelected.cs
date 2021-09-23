using GameLayer.WeaponSystem;

namespace GameLayer.CharacterSystem.CharacterWeaponsSystem
{
    public interface ICharacterWeaponSelected
    {
        IWeapon SelectedWeapon { get; }
    }
}