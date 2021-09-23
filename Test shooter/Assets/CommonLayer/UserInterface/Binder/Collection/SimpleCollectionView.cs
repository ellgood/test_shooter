using System.Collections.Generic;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Pooling;
using CommonLayer.UserInterface.Views;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommonLayer.UserInterface.Binder.Collection
{
    public class SimpleCollectionView : CollectionViewBase
    {
        [SerializeField]
        protected ViewElement _template;

        [SerializeField]
        private Transform _poolRoot;

        [SerializeField]
        private Transform _contentPanel;

        private UnityComponentPool<ViewElement> _pool;

        protected List<ViewElement> Elements { get; } = new List<ViewElement>();

        protected override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(_template);

            Transform t = transform;
            _pool = new UnityComponentPool<ViewElement>(_template, _contentPanel ? _contentPanel : t,
                _poolRoot ? _poolRoot : t);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (!Binder.Initialized || Binder.Count <= 0)
            {
                return;
            }

            for (var i = 0; i < Binder.Count; i++)
            {
                OnItemAdded(i, Binder[i]);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnClear();
        }

        protected override void OnItemReplaced(int idx, BindContext bindContext)
        {
            ViewElement template = Elements[idx];
            template.ResetView();
            template.SetContext(bindContext);
        }

        protected override void OnItemRemoved(int idx)
        {
            ViewElement template = Elements[idx];
            Elements.RemoveAt(idx);

            template.ResetView();
            ReturnViewElement(template);
        }

        protected override void OnItemAdded(int idx, BindContext bindContext)
        {
            ViewElement template = RentViewElement();
            template.SetContext(bindContext);

            Elements.Insert(idx, template);
            template.RectTransform.SetSiblingIndex(idx);
        }

        protected override void OnClear()
        {
            foreach (ViewElement template in Elements)
            {
                template.ResetView();
                ReturnViewElement(template);
            }

            Elements.Clear();
        }

        protected ViewElement RentViewElement()
        {
            return _pool.Rent();
        }

        protected void ReturnViewElement(ViewElement viewElement)
        {
            viewElement.ResetView();
            _pool.Return(viewElement);
        }
    }
}