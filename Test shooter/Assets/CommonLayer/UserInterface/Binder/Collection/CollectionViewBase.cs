using CommonLayer.UserInterface.DataBinding;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace CommonLayer.UserInterface.Binder.Collection
{
    [RequireComponent(typeof(ICollectionBinder))]
    public abstract class CollectionViewBase : UIBehaviour
    {
        protected ICollectionBinder Binder { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Binder = GetComponentInParent<ICollectionBinder>();

            Assert.IsNotNull(Binder);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Binder.ItemAdded += OnItemAdded;
            Binder.ItemRemoved += OnItemRemoved;
            Binder.ItemReplaced += OnItemReplaced;
            Binder.Clear += OnClear;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Binder.ItemAdded -= OnItemAdded;
            Binder.ItemRemoved -= OnItemRemoved;
            Binder.ItemReplaced -= OnItemReplaced;
            Binder.Clear -= OnClear;
        }

        protected abstract void OnItemReplaced(int idx, BindContext bindContext);
        protected abstract void OnItemRemoved(int idx);
        protected abstract void OnItemAdded(int idx, BindContext bindContext);
        protected abstract void OnClear();
    }
}