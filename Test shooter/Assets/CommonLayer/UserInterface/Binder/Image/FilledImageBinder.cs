using CommonLayer.UserInterface.DataBinding;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Image
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public sealed class FilledImageBinder : ViewBinderBase
    {
        public enum FilledDirectionTween
        {
            WithoutTween,
            AnyWay,
            IncreaseWay,
            DecreaseWay
        }

        [SerializeField]
        private string _valueBindKey;

        [SerializeField]
        private FilledDirectionTween _tween = FilledDirectionTween.AnyWay;

        [SerializeField]
        private float _tweenDuration = 1.5f;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private UnityEngine.UI.Image _image;

        protected override void OnBind(BindContext bindContext)
        {
            bindContext.Bind(_valueBindKey, Observer.Create<float>(ValueChanged)).AddTo(_disposable);
        }

        protected override void OnUnbind()
        {
            _disposable.Clear();
        }

        private void Awake()
        {
            _image = GetComponent<UnityEngine.UI.Image>();
        }

        private void ValueChanged(float data)
        {
            float delta = data - _image.fillAmount;

            if (delta > 0 && (_tween == FilledDirectionTween.IncreaseWay || _tween == FilledDirectionTween.AnyWay))
            {
                _image.DOFillAmount(data, _tweenDuration * delta);
            }
            else if (delta < 0 && (_tween == FilledDirectionTween.DecreaseWay || _tween == FilledDirectionTween.AnyWay))
            {
                _image.DOFillAmount(data, _tweenDuration * -delta);
            }
            else
            {
                _image.fillAmount = data;
            }
        }
    }
}