using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(Slider))]
    public sealed class SliderBinder : FillItemBinder
    {
        [SerializeField]
        public string _endDrugSignalKey;

        private Slider _slider;

        protected override void OnBind(BindContext bindContext)
        {
            base.OnBind(bindContext);
            if (!string.IsNullOrWhiteSpace(_endDrugSignalKey))
            {
                bindContext.Trigger(_endDrugSignalKey, _slider.OnPointerUpAsObservable()
                    .Select(a => MapInputToValue(_slider.value))).AddTo(Disposable);
            }
        }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        protected override IObservable<float> GetObservableValue()
        {
            return _slider.onValueChanged.AsObservable().Select(MapInputToValue);
        }

        protected override void OnRefreshValue(float value)
        {
            _slider.SetValueWithoutNotify(value);
        }
    }
}