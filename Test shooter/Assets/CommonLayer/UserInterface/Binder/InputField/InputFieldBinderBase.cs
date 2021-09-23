using CommonLayer.UserInterface.DataBinding;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace CommonLayer.UserInterface.Binder.InputField
{
    [RequireComponent(typeof(TMP_InputField))]
    public abstract class InputFieldBinderBase<T> : ViewBinderBase
    {
        [SerializeField]
        [FormerlySerializedAs("_key")]
        private string _inputTextKey;

        [SerializeField]
        private string _submitKey;

        private CompositeDisposable _disposable;
        private TMP_InputField _inputField;

        protected abstract TMP_InputField.ContentType ContentType { get; }

        private TMP_InputField InputField => _inputField ? _inputField : _inputField = GetComponent<TMP_InputField>();

        protected override void OnBind(BindContext bindContext)
        {
            _disposable = new CompositeDisposable();
            if (!string.IsNullOrWhiteSpace(_inputTextKey))
            {
                bindContext.Bind(_inputTextKey, Observable.CreateWithState<string, TMP_InputField>(InputField, (i, observer) =>
                {
                    observer.OnNext(i.text);
                    return i.onValueChanged.AsObservable().Subscribe(observer);
                }).Select(FromText), Observer.Create<T>(ValueChanged)).AddTo(_disposable);
            }

            if (!string.IsNullOrWhiteSpace(_submitKey))
            {
                bindContext.Trigger(_submitKey, InputField.onSubmit.AsObservable().Select(FromText)).AddTo(_disposable);
            }
        }

        protected override void OnUnbind()
        {
            _disposable.Dispose();
        }

        protected abstract string ToText(T value);

        protected abstract T FromText(string value);

        private void Awake()
        {
            InputField.contentType = ContentType;
        }

        private void ValueChanged(T data)
        {
            InputField.text = ToText(data);
        }
    }
}