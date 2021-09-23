using System;
using System.Collections;
using System.Collections.Generic;
using CommonLayer.UserInterface.Binder.Collection.Delegates;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Views;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Collection
{
    public abstract class DictionaryBinderBase<TKey> : ViewBinderBase, ICollectionBinder
    {
        [SerializeField]
        private string _dictionaryKey;

        private readonly CompositeDisposable _dependentResources;

        private readonly Dictionary<TKey, ViewElement> _templates = new Dictionary<TKey, ViewElement>();
        private SortedList<TKey, BindContext> _bindContexts;

        //private readonly List<BindContext> _bindContexts = new List<BindContext>();
        private ReactiveDictionary<TKey, BindContext> _contextsDictionary;

        protected DictionaryBinderBase()
        {
            _dependentResources = new CompositeDisposable();
        }

        #region ICollectionBinder Implementation

        public event CollectionChangeIdxAction ItemAdded;
        public event CollectionRemoveAction ItemRemoved;
        public event CollectionChangeIdxAction ItemReplaced;
        public event Action Clear;

        public bool Initialized { get; private set; }

        #endregion

        #region IEnumerable Implementation

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable) _bindContexts.Values).GetEnumerator();
        }

        #endregion

        #region IEnumerable<BindContext> Implementation

        IEnumerator<BindContext> IEnumerable<BindContext>.GetEnumerator()
        {
            return _bindContexts.Values.GetEnumerator();
        }

        #endregion

        #region IReadOnlyCollection<BindContext> Implementation

        public int Count => _bindContexts.Count;

        #endregion

        #region IReadOnlyList<BindContext> Implementation

        public BindContext this[int index] => _bindContexts.Values[index];

        #endregion

        protected sealed override void OnBind(BindContext context)
        {
            Init();

            BindDictionaryEvents();
            context.BindDictionary(_dictionaryKey, _contextsDictionary).AddTo(_dependentResources);
        }

        protected virtual IComparer<TKey> GetKeyComparer()
        {
            return Comparer<TKey>.Default;
        }

        protected sealed override void OnUnbind()
        {
            _dependentResources.Clear();
            _contextsDictionary.Clear();
        }

        private void Init()
        {
            if (Initialized)
            {
                return;
            }

            _contextsDictionary = new ReactiveDictionary<TKey, BindContext>();
            _bindContexts = new SortedList<TKey, BindContext>(GetKeyComparer());
            Initialized = true;
        }

        private void BindDictionaryEvents()
        {
            _contextsDictionary.ObserveAdd().Subscribe(OnAdded).AddTo(_dependentResources);
            _contextsDictionary.ObserveRemove().Subscribe(OnRemoved).AddTo(_dependentResources);
            _contextsDictionary.ObserveReplace().Subscribe(OnReplaced).AddTo(_dependentResources);
            _contextsDictionary.ObserveReset().Subscribe(OnReset).AddTo(_dependentResources);
        }

        private void OnReset(Unit obj)
        {
            _bindContexts.Clear();

            Clear?.Invoke();
        }

        private void OnReplaced(DictionaryReplaceEvent<TKey, BindContext> obj)
        {
            int idx = _bindContexts.IndexOfKey(obj.Key);
            _bindContexts.Values[idx] = obj.NewValue;

            ItemReplaced?.Invoke(idx, obj.NewValue);
        }

        private void OnRemoved(DictionaryRemoveEvent<TKey, BindContext> obj)
        {
            int idx = _bindContexts.IndexOfKey(obj.Key);
            _bindContexts.RemoveAt(idx);

            ItemRemoved?.Invoke(idx);
        }

        private void OnAdded(DictionaryAddEvent<TKey, BindContext> obj)
        {
            _bindContexts.Add(obj.Key, obj.Value);
            int idx = _bindContexts.IndexOfKey(obj.Key);

            ItemAdded?.Invoke(idx, obj.Value);
        }
    }
}