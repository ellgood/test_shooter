using CommonLayer.ResourceSystem.Config.Interfaces;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Interface
{
    public interface IGameObjectData<out TConfig>
        where TConfig : IConfig
    { 
        GameObject Prefab { get; }

        TConfig Config { get; }
        
    }
}