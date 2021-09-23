using CommonLayer.ResourceSystem.Config.Interfaces;
using UnityEngine;

namespace GameLayer.DamageSystem
{
    public interface IDamageController
    {
        void CheckOfExpDamage(Collider collider, Vector3 center, float damageMtp, IBulletConfig bulletConfig);
        void CheckOfSimpleDamage(Collider collider, Vector3 normal, float damageMtp, IBulletConfig bulletConfig);
    }
}