namespace GameLayer.EnemySystem
{
    public interface IEnemyController
    {
        bool TryGetEnemy(int hitHash, out IDamagedEnemy damagedEnemy);

        bool CheckAlive(int hitHash);
    }
}