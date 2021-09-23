using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using GameLayer.CharacterSystem.CharacterWeaponsSystem.ShootingSystem;
using GameLayer.WeaponSystem;
using UnityEngine;

namespace GameLayer.CharacterSystem.CharacterWeaponsSystem
{
    public sealed class CharacterWeaponSelectControl : ICharacterWeaponSelectControl, ICharacterWeaponSelected
    {
        private const string ScrollWheelKey = "Mouse ScrollWheel";

        private readonly KeyCode[] _keyCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
    

        private readonly IWeaponSceneController _weaponSceneController;
        private readonly IShootingSetWeaponProvider _setWeaponProvider;
        private IWeaponComponent _component;

        private int _selectedIdx;

        public CharacterWeaponSelectControl(
            IWeaponSceneController weaponSceneController,
            IShootingSetWeaponProvider setWeaponProvider)
        {
            _weaponSceneController = weaponSceneController;
            _setWeaponProvider = setWeaponProvider;
        }

        public void Init(IWeaponComponent component)
        {
            _component = component;
            _selectedIdx = 0;

            SelectByIdx(_selectedIdx);

            _setWeaponProvider.SetWeapon(SelectedWeapon);
        }

        public void Update()
        {
            for (var i = 0; i < _keyCodes.Length; ++i)
            {
                if (!Input.GetKeyDown(_keyCodes[i])) continue;

                ReturnSelected();
                _selectedIdx = i;
                SelectByIdx(_selectedIdx);
                _setWeaponProvider.SetWeapon(SelectedWeapon);
                break;
            }

            var delta = Input.GetAxis(ScrollWheelKey);
            if (delta == 0f) return;

            var flag = delta > 0;
            if (flag)
            {
                SelectNext();
                _setWeaponProvider.SetWeapon(SelectedWeapon);
            }
            else
            {
                SelectPrevious();
                _setWeaponProvider.SetWeapon(SelectedWeapon);
            }
        }

        public IWeapon SelectedWeapon { get; private set; }

        private void SelectByIdx(int idx)
        {
            var weaponType = (WeaponType)idx;
            SelectedWeapon = _weaponSceneController.WeaponAttachTo(weaponType, _component.Slot);
            _component.Slot.localPosition = SelectedWeapon.Data.SlotPosition;
            _component.Muzzle.localPosition = SelectedWeapon.Data.MuzzlePosition;
        }

        private void ReturnSelected()
        {
            if (SelectedWeapon == null) return;
            _component.Slot.localPosition = Vector3.one;
            _component.Muzzle.localPosition = Vector3.one;
            _weaponSceneController.WeaponReturn(SelectedWeapon.Data.WeaponType);
        }

        private void SelectNext()
        {
            ReturnSelected();
            _selectedIdx++;
            if (_selectedIdx >= _weaponSceneController.Count) _selectedIdx = 0;

            SelectByIdx(_selectedIdx);
        }

        private void SelectPrevious()
        {
            ReturnSelected();
            _selectedIdx--;
            if (_selectedIdx < 0) _selectedIdx = _weaponSceneController.Count - 1;

            SelectByIdx(_selectedIdx);
        }
    }
}