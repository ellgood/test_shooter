using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace CommonLayer.UserInterface.Pooling
{
    public class UnityComponentPool<TComponent> : UnityObjectPoolBase<TComponent>
        where TComponent : Component
    {
        private readonly Transform _rentRoot;
        private readonly Transform _root;

#if UNITY_EDITOR && POOL_RENT_ID
        private int _rentCount;
#endif
        private readonly string _tag;
        private readonly TComponent _original;

        public UnityComponentPool(
            string poolTag,
            TComponent original,
            Transform rentRoot = null,
            Transform poolRoot = null) : this(poolTag, rentRoot, poolRoot)
        {
            Assert.IsNotNull(original);
            _original = original;

            GameObject originalGo = original.gameObject;
            if (!originalGo.activeInHierarchy)
            {
                return;
            }

            originalGo.name += "[Template]";
            originalGo.transform.SetParent(_root);
            originalGo.SetActive(false);
        }

        public UnityComponentPool(
            TComponent original,
            Transform rentRoot = null,
            Transform poolRoot = null) : this(original.name, original, rentRoot, poolRoot) { }

        internal UnityComponentPool(string tag, Transform rentRoot = null, Transform poolRoot = null)
        {
            _rentRoot = rentRoot;
            _tag = tag;

            if (poolRoot != null)
            {
                _root = poolRoot;
            }
            else
            {
                _root = new GameObject($"Pool [{tag}]").transform;
                Object.DontDestroyOnLoad(PoolRoot.gameObject);
            }
        }

        public bool MoveToActiveScene { get; protected set; } = true;

        public string Tag => _tag;

        private protected Transform PoolRoot => _root;

        private protected Transform PoolRentRoot => _rentRoot;

        protected override TComponent CreateInstance()
        {
            return Object.Instantiate(_original, Vector3.zero, Quaternion.identity, PoolRoot);
        }

        protected override void PostCreate(TComponent instance)
        {
            base.PostCreate(instance);
            instance.gameObject.SetActive(false);
        }

        protected override void OnBeforeRent(TComponent instance)
        {
            GameObject instanceGo = instance.gameObject;

            if (_rentRoot)
            {
                instance.transform.SetParent(_rentRoot, false);
            }
            else
            {
                instance.transform.SetParent(null);
                if (MoveToActiveScene)
                {
                    SceneManager.MoveGameObjectToScene(instanceGo, SceneManager.GetActiveScene());
                }
            }

#if UNITY_EDITOR && POOL_RENT_ID
            instanceGo.name = $"{_original.name}_{_rentCount:X}";
            _rentCount++;
            RefreshPoolName();
#endif
            if (!instanceGo.activeSelf)
            {
                instanceGo.SetActive(true);
            }
        }

        protected override void OnAfterReturn(TComponent instance)
        {
            instance.gameObject.SetActive(false);

#if UNITY_EDITOR && POOL_RENT_ID
            instance.name += "[Reserved]";
            RefreshPoolName();
#endif

            Transform t = instance.transform;
            t.SetParent(PoolRoot, false);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }

        [Conditional("UNITY_EDITOR")]
        private void RefreshPoolName()
        {
            if (!_root)
            {
                return;
            }
            _root.name = $"Pool [{_tag}] {Count} / {Count + RentedCount} ({RentedCount})";
        }
    }
}