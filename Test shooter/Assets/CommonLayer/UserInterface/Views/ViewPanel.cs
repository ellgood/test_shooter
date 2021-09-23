using System;
using CommonLayer.UserInterface.DataBinding;
using UnityEngine;

namespace CommonLayer.UserInterface.Views
{
    public sealed class ViewPanel : ViewBase
    {
        [SerializeField]
        private ViewElement[] _subViews;

        public override void Hide(Action callback)
        {
            int viewCount = _subViews.Length;
            if (viewCount == 0)
            {
                callback?.Invoke();
            }

            foreach (ViewElement v in _subViews)
            {
                v.Hide(() =>
                {
                    viewCount--;
                    if (viewCount == 0)
                    {
                        callback?.Invoke();
                    }
                });
            }
        }

        public override void Show(Action callback)
        {
            int viewCount = _subViews.Length;
            if (viewCount == 0)
            {
                callback?.Invoke();
            }

            foreach (ViewElement v in _subViews)
            {
                v.Show(() =>
                {
                    viewCount--;
                    if (viewCount == 0)
                    {
                        callback?.Invoke();
                    }
                });
            }
        }

        public override void SetDefault()
        {
            foreach (ViewElement v in _subViews)
            {
                v.SetDefault();
            }
        }

        public override void DropContext()
        {
            foreach (ViewElement v in _subViews)
            {
                v.DropContext();
            }
        }

        public override bool SetContext(BindContext bindContext)
        {
            foreach (ViewElement viewElement in _subViews)
            {
                viewElement.SetContext(bindContext);
            }

            return true;
        }
    }
}