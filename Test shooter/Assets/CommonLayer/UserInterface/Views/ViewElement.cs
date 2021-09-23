using System;
using System.Collections.Generic;
using CommonLayer.UserInterface.Binder;
using CommonLayer.UserInterface.DataBinding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

namespace CommonLayer.UserInterface.Views
{
    public sealed class ViewElement : ViewBase
    {
        private static readonly List<ViewBinderBase> _bindersBuffer = new List<ViewBinderBase>();
        private static readonly List<ViewBinderBase> _bindersInnerBuffer = new List<ViewBinderBase>();
        private static readonly List<ViewElement> _elementBuffer = new List<ViewElement>();

        public UnityEvent BeginHide;
        public UnityEvent EndHiding;
        public UnityEvent BeginShow;
        public UnityEvent EndShowing;

        [SerializeField]
        private bool _autoRefresh;

        [SerializeField]
        private bool _bindAnotherElements;

        private ViewAnimationBase _animation;

        private BindContext _bindContext;

        private bool _binderDirty = true;

        private ViewBinderBase[] _binders = Array.Empty<ViewBinderBase>();
        private ViewElement[] _dependElements = Array.Empty<ViewElement>();

        private bool _elementBound;

        protected override void Awake()
        {
            base.Awake();

            CheckBinders();

            if (!_animation)
            {
                _animation = GetComponent<ViewAnimationBase>();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DropContext();
        }

        private void OnTransformChildrenChanged()
        {
            if (_autoRefresh)
            {
                _binderDirty = true;
            }
        }

        public override void Hide(Action callback)
        {
            BeginHide.Invoke();

            void Callback()
            {
                callback?.Invoke();
                EndHiding.Invoke();
            }

            if (_animation != null)
            {
                _animation.HideAnimation(Callback);
            }
            else
            {
                Callback();
            }
        }

        public override void Show(Action callback)
        {
            BeginShow.Invoke();

            void Callback()
            {
                callback?.Invoke();
                EndShowing.Invoke();
            }

            if (_animation != null)
            {
                _animation.ShowAnimation(Callback);
            }
            else
            {
                Callback();
            }
        }

        public override void SetDefault()
        {
            if (_animation != null)
            {
                _animation.ResetAnimation();
            }
        }

        [ContextMenu("Refresh binders")]
        public void ForceRefresh()
        {
            _binderDirty = true;
            CheckBinders();
        }

        public override bool SetContext(BindContext bindContext)
        {
            if (bindContext == null)
            {
                return false;
            }

            _bindContext = bindContext;
            CheckBinders();

            BindElements(_bindContext);
            Bind(_bindContext);

            return true;
        }

        public override void DropContext()
        {
            if (_bindContext == null)
            {
                return;
            }

            Unbind();
            UnbindElements();

            _bindContext = null;
        }

        private void CheckBinders()
        {
            if (_binderDirty)
            {
                RefreshBinders();
                _binderDirty = false;
            }
        }

        private void RefreshBinders()
        {
            try
            {
                Profiler.BeginSample(nameof(RefreshBinders));

                FindAllBinders(transform, _bindersBuffer, _elementBuffer);

                Profiler.EndSample();

                if (_bindContext != null)
                {
                    Unbind();
                    UnbindElements();

                    _binders = _bindersBuffer.ToArray();
                    _dependElements = _elementBuffer.ToArray();

                    BindElements(_bindContext);
                    Bind(_bindContext);
                }
                else
                {
                    _binders = _bindersBuffer.ToArray();
                    _dependElements = _elementBuffer.ToArray();
                }
            }
            finally
            {
                _bindersBuffer.Clear();
                _elementBuffer.Clear();
            }
        }

        private void FindAllBinders(Transform t, ICollection<ViewBinderBase> binders, ICollection<ViewElement> elements)
        {
            var viewElement = t.GetComponent<ViewElement>();
            if (viewElement && viewElement != this)
            {
                elements.Add(viewElement);
                return;
            }

            t.GetComponents(_bindersInnerBuffer);

            foreach (ViewBinderBase viewBinderBase in _bindersInnerBuffer)
            {
                binders.Add(viewBinderBase);
            }

            for (var i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                FindAllBinders(child, binders, elements);
            }
        }

        private void BindElements(BindContext bindContext)
        {
            if (!_bindAnotherElements)
            {
                return;
            }

            foreach (ViewElement dependElement in _dependElements)
            {
                dependElement.SetContext(bindContext);
            }

            _elementBound = true;
        }

        private void UnbindElements()
        {
            if (_elementBound)
            {
                foreach (ViewElement dependElement in _dependElements)
                {
                    dependElement.DropContext();
                }

                _elementBound = false;
            }
        }

        private void Bind(BindContext bindContext)
        {
            foreach (ViewBinderBase newViewBinderBase in _binders)
            {
                newViewBinderBase.Bind(bindContext);
            }
        }

        private void Unbind()
        {
            foreach (ViewBinderBase newViewBinderBase in _binders)
            {
                newViewBinderBase.Unbind();
            }
        }
    }
}