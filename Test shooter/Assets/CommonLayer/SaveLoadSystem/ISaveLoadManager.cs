namespace CommonLayer.SaveLoadSystem
{
    public interface ISaveLoadManager
    {
        void Save<TData>(string key, TData data)
            where TData : class;

        bool TryLoad<TData>(string key, out TData data)
            where TData : class;

    }
}