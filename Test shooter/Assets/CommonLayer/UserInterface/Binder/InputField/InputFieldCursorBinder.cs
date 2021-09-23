using CommonLayer.UserInterface.DataBinding;
using TMPro;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.InputField
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldCursorBinder : ViewBinderBase
    {
        [SerializeField]
        private string _endLineSignalKey;

        [SerializeField]
        private string _endTextSignalKey;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private TMP_InputField _inputField;

        protected override void OnBind(BindContext bindContext)
        {
            if (!string.IsNullOrWhiteSpace(_endLineSignalKey))
            {
                bindContext.Signal(_endLineSignalKey, OnEndLineSignal).AddTo(_disposable);
            }

            if (!string.IsNullOrWhiteSpace(_endTextSignalKey))
            {
                bindContext.Signal(_endTextSignalKey, OnEndTextSignal).AddTo(_disposable);
            }
        }

        protected override void OnUnbind()
        {
            _disposable.Clear();
        }

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        private void OnEndTextSignal()
        {
            _inputField.MoveTextEnd(false);
        }

        private void OnEndLineSignal()
        {
            _inputField.MoveToEndOfLine(false, false);
        }
    }
}