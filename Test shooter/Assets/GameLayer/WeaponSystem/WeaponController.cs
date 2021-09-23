using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace GameLayer.WeaponSystem
{
    
    
    
    public class WeaponController : IWeaponContent, IWeaponSwitch
    {
        private readonly IWeaponDataBase _dataBase;
        private readonly int _weaponCount;
        private readonly List<IWeapon> _weapons = new List<IWeapon>();
  
        private int _selectedIdx;

        public WeaponController(IWeaponDataBase dataBase)
        {
            _dataBase = dataBase;


            // todo to init

            FillWeaponList(_weapons, dataBase);
            
            _weaponCount = _weapons.Count;
            SelectByIdx(0); // or loaded weapon
        }

        private void FillWeaponList(List<IWeapon> weapons, IWeaponDataBase dataBase)
        {
            Assert.IsNotNull(weapons);
            Assert.IsNotNull(dataBase);
            //TODO Fill weapons
        }

        public IWeapon CurrentWeapon { get; private set; }
        public IReadOnlyList<IWeapon> Weapons => _weapons;

        public void SelectNext()
        {
            _selectedIdx++;
            if (_selectedIdx >= _weaponCount) _selectedIdx = 0;
            CurrentWeapon = _weapons[_selectedIdx];
        }

        public void SelectPrevious()
        {
            _selectedIdx--;
            if (_selectedIdx < 0) _selectedIdx = _weaponCount - 1;
            CurrentWeapon = _weapons[_selectedIdx];
        }

        public void SelectByIdx(int idx)
        {
            if (idx > _weaponCount - 1 || idx < 0)
                throw new IndexOutOfRangeException($"Weapon Idx: {idx} out of weapon range");
            _selectedIdx = idx;
            CurrentWeapon = _weapons[_selectedIdx];
        }
    }
}