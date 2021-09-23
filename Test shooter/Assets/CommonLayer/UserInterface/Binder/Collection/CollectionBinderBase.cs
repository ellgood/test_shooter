using System;
using System.Collections;
using System.Collections.Generic;
using CommonLayer.UserInterface.Binder.Collection.Delegates;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Collection
{
    public abstract class CollectionBinderBase : ViewBinderBase, ICollectionBinder
    {
        [SerializeField]
        private string _listKey;

        private readonly ReactiveCollection<BindContext> _contextsCollection = new ReactiveCollection<BindContext>();

        private readonly CompositeDisposable _dependentResources;

        protected CollectionBinderBase()
        {
            _dependentResources = new CompositeDisposable();
        }

        #region ICollectionBinder Implementation

        public event CollectionChangeIdxAction ItemAdded;
        public event CollectionRemoveAction ItemRemoved;
        public event CollectionChangeIdxAction ItemReplaced;
        public event Action Clear;

        public bool Initialized => true;

        #endregion

        #region IEnumerable Implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _contextsCollection).GetEnumerator();
        }

        #endregion

        #region IEnumerable<BindContext> Implementation

        public IEnumerator<BindContext> GetEnumerator()
        {
            return _contextsCollection.GetEnumerator();
        }

        #endregion

        #region IReadOnlyCollection<BindContext> Implementation

        public int Count => _contextsCollection.Count;

        #endregion

        #region IReadOnlyList<BindContext> Implementation

        public BindContext this[int index] => _contextsCollection[index];

        #endregion

        protected override void OnBind(BindContext context)
        {
            BindCollectionEvents();
            context.BindCollection(_listKey, _contextsCollection).AddTo(_dependentResources);
        }

        protected override void OnUnbind()
        {
            _dependentResources.Clear();

            if (_contextsCollection.Count <= 0)
            {
                return;
            }

            _contextsCollection.Clear();
            Clear?.Invoke();
            OnReset();
        }

        protected virtual void OnReset() { }
        protected virtual void OnReplace(CollectionReplaceEvent<BindContext> collectionReplaceEvent) { }
        protected virtual void OnRemove(CollectionRemoveEvent<BindContext> collectionRemoveEvent) { }
        protected virtual void OnMove(CollectionMoveEvent<BindContext> collectionMoveEvent) { }
        protected virtual void OnAdd(CollectionAddEvent<BindContext> collectionAddEvent) { }

        private void BindCollectionEvents()
        {
            _contextsCollection.ObserveAdd().Subscribe(e =>
                {
                    ItemAdded?.Invoke(e.Index, e.Value);
                    OnAdd(e);
                })
                .AddTo(_dependentResources);
            _contextsCollection.ObserveMove().Subscribe(e =>
            {
                ItemRemoved?.Invoke(e.OldIndex);
                ItemAdded?.Invoke(e.NewIndex, e.Value);
                OnMove(e);
            }).AddTo(_dependentResources);
            _contextsCollection.ObserveRemove().Subscribe(e =>
                {
                    ItemRemoved?.Invoke(e.Index);
                    OnRemove(e);
                })
                .AddTo(_dependentResources);
            _contextsCollection.ObserveReplace().Subscribe(e =>
                {
                    ItemReplaced?.Invoke(e.Index, e.NewValue);
                    OnReplace(e);
                })
                .AddTo(_dependentResources);
            _contextsCollection.ObserveReset().Subscribe(e =>
                {
                    Clear?.Invoke();
                    OnReset();
                })
                .AddTo(_dependentResources);
        }
    }
}