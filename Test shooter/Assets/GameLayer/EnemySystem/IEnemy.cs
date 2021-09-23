namespace GameLayer.EnemySystem
{
    public interface IEnemy : IDamagedEnemy
    {
        int Hash { get; }
        void Destroy();
    }
}