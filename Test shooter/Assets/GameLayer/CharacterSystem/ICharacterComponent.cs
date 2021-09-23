using GameLayer.WeaponSystem;
using UnityEngine;

namespace GameLayer.CharacterSystem
{
    public interface ICharacterComponent
    {
        Transform LookSlotTransform { get; }

        Transform BodySlotTransform { get; }

        Transform CharacterSlotTransform { get; }

        CharacterController Controller { get; }
        
        IWeaponComponent WeaponComponent { get; }
    }
}