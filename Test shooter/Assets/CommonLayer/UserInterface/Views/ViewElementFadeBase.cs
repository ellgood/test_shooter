using System;
using DG.Tweening;
using UnityEngine;

namespace CommonLayer.UserInterface.Views
{
    public abstract class ViewElementFadeBase : ViewAnimationBase
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private float _showTime = 0.5f;

        [SerializeField]
        private float _hideTime = 0.25f;

        [SerializeField]
        private Ease _showEase = Ease.Linear;

        [SerializeField]
        private Ease _hideEase = Ease.Linear;

        public override void ShowAnimation(Action showedCallback)
        {
            _canvasGroup.alpha = 0;
            OnShowAnimation();
            _canvasGroup.DOFade(1, _showTime).SetEase(_showEase).SetDelay(0.04f).OnComplete(() => showedCallback());
        }

        public override void HideAnimation(Action hiddenCallback)
        {
            _canvasGroup.DOFade(0, _hideTime).SetEase(_hideEase).OnComplete(() =>
            {
                OnHideAnimation();
                hiddenCallback();
            });
        }

        public override void ResetAnimation(bool hidden = true)
        {
            _canvasGroup.alpha = hidden ? 0 : 1;
            OnResetAnimation();
        }

        protected virtual void OnShowAnimation() { }
        protected virtual void OnHideAnimation() { }
        protected virtual void OnResetAnimation() { }
    }
}