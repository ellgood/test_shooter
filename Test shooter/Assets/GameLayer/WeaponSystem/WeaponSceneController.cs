using System;
using System.Collections.Generic;
using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using CommonLayer.ResourceSystem.Impl;
using CommonLayer.ResourceSystem.Interface;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace GameLayer.WeaponSystem
{
    public sealed class WeaponSceneController : IWeaponSceneController, IInitializable, IDisposable
    {
        private const string ParentSlotKey = "[PoolWeapons]";

        private readonly IResourcesController _resourcesController;
        private readonly WeaponFactory _weaponFactory;
        private readonly Dictionary<WeaponType, IWeapon> _weapons = new Dictionary<WeaponType, IWeapon>();
        private Transform _parent;


        public WeaponSceneController(
            IResourcesController resourcesController, 
            WeaponFactory weaponFactory)
        {
            _resourcesController = resourcesController;
            _weaponFactory = weaponFactory;
        }

        public void Dispose()
        {
            foreach (var kvp in _weapons) WeaponReturn(kvp.Value.Data.WeaponType);
            _weapons.Clear();
        }

        public void Initialize()
        {
            var weapons = _resourcesController.GetData<WeaponsResourcesData>().Content;

            if (_parent == null)
            {
                var obj = new GameObject(ParentSlotKey);
                _parent = obj.transform;
                Object.DontDestroyOnLoad(obj);
            }

            _weapons.Add(weapons.PistolData.WeaponType, Create(weapons.PistolData));
            _weapons.Add(weapons.MachineGunData.WeaponType, Create(weapons.MachineGunData));
            _weapons.Add(weapons.RocketLauncherData.WeaponType, Create(weapons.RocketLauncherData));
        }

        public void WeaponReturn(WeaponType type)
        {
            WeaponAttachTo(type, _parent, false);
        }

        public int Count => _weapons.Count;

        public IWeapon WeaponAttachTo(WeaponType type, Transform parent, bool isActive = true)
        {
            var weapon = _weapons[type];
            var transform = weapon.Instance.transform;
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            weapon.Instance.SetActive(isActive);
            return weapon;
        }

        private IWeapon Create(WeaponData data)
        {
            var weapon = _weaponFactory.Create(Vector3.zero, _parent, data);
            weapon.Instance.SetActive(false);
            return weapon;
        }
    }
}