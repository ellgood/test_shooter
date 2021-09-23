using System;
using CommonLayer.UserInterface.DataBinding;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CommonLayer.UserInterface.Views
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ViewBase : UIBehaviour
    {
        private RectTransform _rect;

        public RectTransform RectTransform
        {
            get
            {
                if (_rect == null)
                {
                    _rect = GetComponent<RectTransform>();
                }

                return _rect;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _rect = GetComponent<RectTransform>();
        }

        public abstract bool SetContext(BindContext bindContext);

        public abstract void DropContext();

        public abstract void Show(Action callback);

        public abstract void Hide(Action callback);

        public abstract void SetDefault();

        public void ResetView()
        {
            DropContext();
            SetDefault();
        }
    }
}
