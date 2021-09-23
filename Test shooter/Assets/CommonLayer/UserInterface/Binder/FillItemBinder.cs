
using System;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Utility;
using UniRx;
using UnityEngine;


namespace CommonLayer.UserInterface.Binder
{
    public abstract class FillItemBinder : ViewBinderBase
    {
        [SerializeField]
        private string _valueBindKey;

        [SerializeField]
        private string _maxValueBindKey;

        [SerializeField]
        private string _minValueBindKey;

        [SerializeField]
        private string _deltaValueBindKey;

        [SerializeField]
        private string _valueOutputKey;

        [SerializeField]
        private float _minFillValue;

        [SerializeField]
        private float _maxFillValue = 1;

        private readonly FloatReactiveProperty _valueTextOutput = new FloatReactiveProperty();
        protected readonly CompositeDisposable Disposable = new CompositeDisposable();

        private bool _rangeIsValid = true;

        protected float Delta { get; private set; }
        protected float MinValue { get; private set; }
        protected float MaxValue { get; private set; } = 1;
        protected float Value { get; private set; }

        private void LateUpdate()
        {
            if (Delta <= 0)
            {
                return;
            }

            Value = Mathf.Min(Value + Delta * Time.deltaTime, MaxValue);
            RefreshValue();
        }

        protected override void OnBind(BindContext bindContext)
        {
            if (!string.IsNullOrWhiteSpace(_minValueBindKey))
            {
                bindContext.Bind(_minValueBindKey, Observer.Create<float>(MinValueChanged)).AddTo(Disposable);
            }

            if (!string.IsNullOrWhiteSpace(_maxValueBindKey))
            {
                bindContext.Bind(_maxValueBindKey, Observer.Create<float>(MaxValueChanged)).AddTo(Disposable);
            }

            if (!string.IsNullOrWhiteSpace(_deltaValueBindKey))
            {
                bindContext.Bind(_deltaValueBindKey, Observer.Create<float>(OnChangeDx)).AddTo(Disposable);
            }

            if (!string.IsNullOrWhiteSpace(_valueBindKey))
            {
                bindContext.Bind(_valueBindKey, GetObservableValue(), Observer.Create<float>(ValueChanged))
                    .AddTo(Disposable);
            }

            if (!string.IsNullOrWhiteSpace(_valueOutputKey))
            {
                bindContext.TextData(_valueOutputKey, _valueTextOutput).AddTo(Disposable);
            }
        }

        protected virtual IObservable<float> GetObservableValue()
        {
            return Observable.Empty<float>();
        }

        protected override void OnUnbind()
        {
            Disposable.Clear();

            Value = 0;

            MinValue = 0;
            MaxValue = 1;

            Delta = 0;
        }

        private void MinValueChanged(float data)
        {
            MinValue = data;

            _rangeIsValid = MaxValue - MinValue > Mathf.Epsilon;
            RefreshValue();
        }

        private void MaxValueChanged(float data)
        {
            MaxValue = data;

            _rangeIsValid = MaxValue - MinValue > Mathf.Epsilon;
            RefreshValue();
        }

        private void ValueChanged(float data)
        {
            Value = data;

            RefreshValue();
        }

        private void RefreshValue()
        {
            if (!_rangeIsValid)
            {
                return;
            }

            float mappedValue = MathUtils.Map(Value, MinValue, MaxValue, _minFillValue, _maxFillValue);
            _valueTextOutput.Value = Value;
            OnRefreshValue(mappedValue);
        }

        private void OnChangeDx(float obj)
        {
            Delta = obj;
        }

        protected abstract void OnRefreshValue(float value);

        protected float MapInputToValue(float inputValue)
        {
            return MathUtils.Map(inputValue, _minFillValue, _maxFillValue, MinValue, MaxValue);
        }
    }
}