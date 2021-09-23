using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace GameLayer.WeaponSystem
{
    public interface IWeapon
    {
        GameObject Instance { get; }
        WeaponData Data { get; }
    }
}