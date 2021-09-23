using UnityEngine;

namespace GameLayer.WeaponSystem
{
    public class WeaponComponent : MonoBehaviour, IWeaponComponent
    {
        [SerializeField] 
        private Transform slot;
        
        [SerializeField] 
        private Transform body;

        [SerializeField] 
        private Transform muzzle;

        [SerializeField] 
        private Transform aim;


        public Transform Slot => slot;

        public Transform Body => body;

        public Transform Muzzle => muzzle;

        public Transform Aim => aim;
    }
}