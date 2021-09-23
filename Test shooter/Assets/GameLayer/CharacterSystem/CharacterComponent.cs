using GameLayer.WeaponSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameLayer.CharacterSystem
{
    public sealed class CharacterComponent : MonoBehaviour, ICharacterComponent
    {
        [SerializeField] 
        private Transform headSlotTransform;
        [SerializeField] 
        private Transform bodySlotTransform;
        [SerializeField] 
        private Transform characterSlotTransform;
        [SerializeField] 
        private CharacterController characterController;

        [SerializeField] 
        private WeaponComponent weaponComponent;

        public Transform LookSlotTransform => headSlotTransform;

        public Transform BodySlotTransform => bodySlotTransform;

        public Transform CharacterSlotTransform => characterSlotTransform;

        public CharacterController Controller => characterController;

        public IWeaponComponent WeaponComponent => weaponComponent;

        private void Awake()
        {
            Assert.IsNotNull(headSlotTransform);
            Assert.IsNotNull(bodySlotTransform);
            Assert.IsNotNull(characterController);
            Assert.IsNotNull(characterSlotTransform);
        }
    }
}