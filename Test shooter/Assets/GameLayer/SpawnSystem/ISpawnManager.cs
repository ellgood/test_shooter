namespace GameLayer.SpawnSystem
{
    public interface ISpawnManager
    {
        bool TryGetFreeSpawnPoint(SpawnPointFlags pointFlags, out SpawnPointInfo point);
    }
}