using System.Collections.Generic;
using UnityEngine;

namespace CommonLayer.UserInterface.Pooling.AutoTrimming
{
    internal sealed class PoolTrimManager : MonoBehaviour
    {
        private static PoolTrimManager _instance;
        private readonly List<IPoolTrimmer> _poolsToCheck = new List<IPoolTrimmer>();

        public static PoolTrimManager Instance => _instance ? _instance : _instance = CreateInstance();

        public void Add(IPoolTrimmer trimmer)
        {
            _poolsToCheck.Add(trimmer);
        }

        public void Remove(IPoolTrimmer trimmer)
        {
            _poolsToCheck.Remove(trimmer);
        }

        private static PoolTrimManager CreateInstance()
        {
            var poolTrimManagerGo = new GameObject("[POOL_TRIM_MANAGER]")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            DontDestroyOnLoad(poolTrimManagerGo);

            return poolTrimManagerGo.AddComponent<PoolTrimManager>();
        }

        private void LateUpdate()
        {
            foreach (IPoolTrimmer trimmer in _poolsToCheck)
            {
                trimmer.Check();
            }
        }
    }
}