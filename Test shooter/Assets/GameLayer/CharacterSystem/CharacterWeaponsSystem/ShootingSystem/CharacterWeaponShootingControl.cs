using CommonLayer.ResourceSystem.Config.Interfaces;
using CommonLayer.ResourceSystem.Data;
using CommonLayer.ResourceSystem.Impl;
using CommonLayer.ResourceSystem.Interface;
using GameLayer.DamageSystem;
using GameLayer.GameParticleSystem;
using GameLayer.WeaponSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameLayer.CharacterSystem.CharacterWeaponsSystem.ShootingSystem
{
    public class CharacterWeaponShootingControl : ICharacterWeaponShootingControl, IShootingSetWeaponProvider
    {
        private const int MaxColliders = 10;
        private const string Fire1Key = "Fire1";
        private const string Fire2Key = "Fire2";
        private readonly IDamageController _damageController;

        private readonly IExplodeParticleController _explodeParticleController;
        private readonly IMuzzleParticleController _muzzleParticleController;
        private readonly IResourcesController _resourcesController;
        private readonly ISimpleParticleController _simpleParticleController;
        private BulletsData _bulletsData;
        private IWeaponConfig _config;

        private float _fireTime;
        private IWeapon _weapon;
        private IWeaponComponent _weaponComponent;

        public CharacterWeaponShootingControl(
            IResourcesController resourcesController,
            IDamageController damageController,
            IExplodeParticleController explodeParticleController,
            ISimpleParticleController simpleParticleController,
            IMuzzleParticleController muzzleParticleController)
        {
            _resourcesController = resourcesController;
            _damageController = damageController;
            _explodeParticleController = explodeParticleController;
            _simpleParticleController = simpleParticleController;
            _muzzleParticleController = muzzleParticleController;
        }

        public void Init(IWeaponComponent component)
        {
            _bulletsData = _resourcesController.GetData<BulletsResourcesData>().Content;
            _weaponComponent = component;
        }

        public void Update()
        {
            if (Input.GetButton(Fire1Key))
            {
                if (!(Time.time >= _fireTime)) return;
                _fireTime = Time.time + 1f / _config.FireRate;
                Shoot(_muzzleParticleController, _simpleParticleController, _bulletsData.Simple);
            }
            else if (Input.GetButton(Fire2Key))
            {
                if (!(Time.time >= _fireTime)) return;
                _fireTime = Time.time + 1f / _config.FireRate;
                Shoot(_muzzleParticleController, _explodeParticleController, _bulletsData.Explosive);
            }
        }

        public void SetWeapon(IWeapon weapon)
        {
            _fireTime = 0;
            _weapon = weapon;
            _config = _weapon.Data.Config;
            Assert.IsNotNull(_weapon);
            Assert.IsNotNull(_weaponComponent);
        }

        private void Shoot(IParticleController particleMuzzle, IParticleController particleHit,
            IBulletConfig bulletConfig)
        {
            var mPos = _weaponComponent.Muzzle.position;
            var mDir = _weaponComponent.Muzzle.forward;

            particleMuzzle.Play(mPos, mDir);
            var config = _weapon.Data.Config;
            var ainPos = _weaponComponent.Aim.position;
            var aimDir = _weaponComponent.Aim.forward;

            if (!Physics.Raycast(ainPos, aimDir,
                out var hit, config.Range))
                return;
            particleHit.Play(hit.point, hit.normal);

            if (bulletConfig.ExplosionRadius > 0)
            {
                var hitColliders = new Collider[MaxColliders];

                var size = Physics.OverlapSphereNonAlloc(hit.point, bulletConfig.ExplosionRadius, hitColliders);
                for (var i = 0; i < size; i++)
                {
                    var collider = hitColliders[i];
                    _damageController.CheckOfExpDamage(collider, hit.point, config.DamageMtp, bulletConfig);
                }
            }
            else
            {
                _damageController.CheckOfSimpleDamage(hit.collider, hit.normal, config.DamageMtp, bulletConfig);
            }
        }
    }
}