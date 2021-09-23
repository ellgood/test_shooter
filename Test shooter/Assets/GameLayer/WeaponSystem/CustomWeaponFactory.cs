using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;
using Zenject;

namespace GameLayer.WeaponSystem
{
    public sealed class CustomWeaponFactory : IFactory<Vector3,Transform,WeaponData,IWeapon>
    {
        private readonly DiContainer _container;

        public CustomWeaponFactory(DiContainer container)
        {
            _container = container;
        }
        public IWeapon Create(Vector3 position, Transform parent, WeaponData data)
        {
            var gameObject = _container.InstantiatePrefab(data.Prefab, position, Quaternion.identity, parent.transform);
            return _container.Instantiate<Weapon>(new object[] {gameObject,data});
        }
    }
}