using System;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Utility.ExecutionOrder;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace CommonLayer.UserInterface.Binder.Collection
{
    [RequireComponent(typeof(CollectionBinder))]
    [ExecuteAfter(typeof(CollectionBinder))]
    [ExecuteAfter(typeof(CollectionViewBase))]
    public sealed class CollectionBinderEvent : MonoBehaviour
    {
        [SerializeField]
        private CollectionIdxValueEvent _addedItem;

        [SerializeField]
        private CollectionIdxEvent _removedItem;

        [SerializeField]
        private CollectionIdxValueEvent _replacedItem;

        [SerializeField]
        private UnityEvent _clearedItems;

        private CollectionBinder _collectionBinder;

        private void Awake()
        {
            _collectionBinder = GetComponent<CollectionBinder>();

            Assert.IsNotNull(_collectionBinder);
        }

        private void OnEnable()
        {
            _collectionBinder.Clear += OnClear;
            _collectionBinder.ItemAdded += OnItemAdded;
            _collectionBinder.ItemRemoved += OnTimeRemoved;
            _collectionBinder.ItemReplaced += OnItemReplaced;
        }

        private void OnDisable()
        {
            _collectionBinder.Clear -= OnClear;
            _collectionBinder.ItemAdded -= OnItemAdded;
            _collectionBinder.ItemRemoved -= OnTimeRemoved;
            _collectionBinder.ItemReplaced -= OnItemReplaced;
        }

        private void OnItemReplaced(int idx, BindContext bindContext)
        {
            _replacedItem.Invoke(idx, bindContext);
        }

        private void OnTimeRemoved(int idx)
        {
            _removedItem.Invoke(idx);
        }

        private void OnItemAdded(int idx, BindContext bindContext)
        {
            _addedItem.Invoke(idx, bindContext);
        }

        private void OnClear()
        {
            _clearedItems.Invoke();
        }

        [Serializable]
        public sealed class CollectionIdxValueEvent : UnityEvent<int, BindContext> { }

        [Serializable]
        public sealed class CollectionIdxEvent : UnityEvent<int> { }
    }
}