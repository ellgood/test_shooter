using System;
using CommonLayer.DataContext.Settings;
using CommonLayer.ResourceSystem.Impl;
using CommonLayer.ResourceSystem.Interface;
using GameLayer.CharacterSystem.CharacterLookSystem;
using GameLayer.CharacterSystem.CharacterMovementSystem;
using GameLayer.CharacterSystem.CharacterWeaponsSystem;
using GameLayer.CharacterSystem.CharacterWeaponsSystem.ShootingSystem;
using GameLayer.GameCameraSystem;
using GameLayer.SpawnSystem;
using UnityEngine;
using Zenject;

namespace GameLayer.CharacterSystem
{
    public sealed class GameCharacterController : IGameCharacterController, IInitializable, IDisposable, ITickable,
        IFixedTickable
    {
        private readonly CharacterFactory _characterFactory;
        private readonly ICharacterLookControl _characterLookControl;
        private readonly ICharacterMovementControl _characterMovementControl;
        private readonly ICharacterSettingsDataContext _characterSettingsCtx;
        private readonly ICharacterWeaponSelectControl _characterWeaponSelectControl;
        private readonly ICharacterWeaponShootingControl _characterWeaponShootingControl;
        private readonly IGameCameraHolder _gameCameraHolder;
        private readonly IResourcesController _resourcesController;

        private readonly ISpawnManager _spawnManager;
        private ICharacter _character;
        private bool _isActive;

        public GameCharacterController(
            ICharacterSettingsDataContext characterSettingsCtx,
            IResourcesController resourcesController,
            CharacterFactory characterFactory,
            ISpawnManager spawnManager,
            ICharacterLookControl characterLookControl,
            ICharacterMovementControl characterMovementControl,
            ICharacterWeaponSelectControl characterWeaponSelectControl,
            ICharacterWeaponShootingControl characterWeaponShootingControl,
            IGameCameraHolder gameCameraHolder)
        {
            _characterSettingsCtx = characterSettingsCtx;
            _resourcesController = resourcesController;
            _characterFactory = characterFactory;

            _spawnManager = spawnManager;
            _characterLookControl = characterLookControl;
            _characterMovementControl = characterMovementControl;
            _characterWeaponSelectControl = characterWeaponSelectControl;
            _characterWeaponShootingControl = characterWeaponShootingControl;
            _gameCameraHolder = gameCameraHolder;
        }

        public void Dispose()
        {
            SetActive(false);
            _gameCameraHolder.ResetParent();
        }

        public void FixedTick()
        {
            if (!_isActive) return;
            _characterWeaponShootingControl.Update();
        }

        public void SetActive(bool state)
        {
            _isActive = state;
            if (_isActive)
            {
                var isVisible = _characterSettingsCtx.CursorVisible;
                Cursor.lockState = !isVisible ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = isVisible;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void Initialize()
        {
            var characterData = _resourcesController.GetData<CharacterResourcesData>().Content;
            if (!_spawnManager.TryGetFreeSpawnPoint(SpawnPointFlags.Character, out var point)) return;
            _character = _characterFactory.Create(characterData.Prefab, point.Position);
            _characterLookControl.Init(_character.CharacterComponent, _characterSettingsCtx);
            _characterMovementControl.Init(_character.CharacterComponent, _characterSettingsCtx);
            _characterWeaponShootingControl.Init(_character.CharacterComponent.WeaponComponent);
            _characterWeaponSelectControl.Init(_character.CharacterComponent.WeaponComponent);
            _gameCameraHolder.SetParent(_character.CharacterComponent.LookSlotTransform);
            SetActive(true);
        }

        public void Tick()
        {
            if (!_isActive) return;
            _characterLookControl.Update();
            _characterMovementControl.Update();
            _characterWeaponSelectControl.Update();
        }
    }
}