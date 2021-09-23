using System;
using UnityEngine;

namespace CommonLayer.SceneControllers
{
    [Serializable]
    public sealed class SceneLoaderData
    {
        [SerializeField]
        private string _viewLoadingKey;

        [SerializeField]
        private float _delayStartLoading;

        [SerializeField]
        private float _delayEndLoading;

        public string ViewLoadingKey => _viewLoadingKey;
        public float DelayStartLoading => _delayStartLoading;
        public float DelayEndLoading => _delayEndLoading;
    }
}