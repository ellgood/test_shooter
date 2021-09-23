using CommonLayer.ResourceSystem.Interface;
using UnityEngine;

namespace CommonLayer.ResourceSystem
{
    public abstract class ResourcesDataBase<TContent> : ScriptableObject, IResourceData<TContent>
        where TContent : class,new()
    {
        public abstract TContent Content { get; }
    }
}