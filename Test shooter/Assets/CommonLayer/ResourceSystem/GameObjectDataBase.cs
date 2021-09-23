using System;
using CommonLayer.ResourceSystem.Config;
using CommonLayer.ResourceSystem.Config.Interfaces;
using CommonLayer.ResourceSystem.Interface;
using UnityEngine;

namespace CommonLayer.ResourceSystem
{
    [Serializable]
    public abstract class GameObjectDataBase<TCConfig,TIConfig> :IGameObjectData<TIConfig>
        where TIConfig : IConfig
        where TCConfig : TIConfig, new()
    
    {
        [SerializeField] 
        private GameObject prefab;
        
        [SerializeField] 
        private TCConfig config;
        public GameObject Prefab => prefab;

        public TIConfig Config => config;
    }
}