using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CommonLayer.SaveLoadSystem
{
    public class SaveLoadManager : ISaveLoadManager
    {
        public void Save<TData>(string key, TData data)
            where TData : class
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + $"/{key}.game";
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public bool TryLoad<TData>(string key, out TData data)
            where TData : class
        {
            var path = Application.persistentDataPath + $"/{key}.game";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                data = formatter.Deserialize(stream) as TData;
                stream.Close();
                return true;
            }

            data = default;
            return false;
        }
    }
}