namespace CommonLayer.SceneControllers
{
    public interface ILockable
    {
        bool Lock(object lockObj);

        bool Unlock(object lockObj);
    }
}