

using UnityEngine;

namespace GameLayer.EnemySystem
{
    public interface IDamagedEnemy
    {
        void TakeDamage(float damage);
        void AddExplosionForce(float force, Vector3 centerPos, float radius);
        bool IsAlive { get;}
        void AddForce(float force, Vector3 normal);
    }
}