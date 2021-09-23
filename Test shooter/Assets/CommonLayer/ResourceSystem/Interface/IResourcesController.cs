using UnityEngine;

namespace CommonLayer.ResourceSystem.Interface
{
    public interface IResourcesController
    {
        T GetData<T>()
            where T : Object, IResourceData;

        bool TryGetData<T>(out T value)
            where T : Object, IResourceData;

        T[] GetDataSet<T>()
            where T : Object, IResourceData;
    }
}