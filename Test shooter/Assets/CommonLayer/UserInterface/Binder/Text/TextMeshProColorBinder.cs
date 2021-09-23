using System;
using CommonLayer.UserInterface.DataBinding;
using TMPro;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Text
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class TextMeshProColorBinder : ViewBinderBase
    {
        [SerializeField]
        private string _colorKey;

        private IDisposable _singleDisposable;

        private TextMeshProUGUI _text;

        protected override void OnBind(BindContext bindContext)
        {
            _singleDisposable = bindContext.Bind(_colorKey, Observer.Create<Color>(ColorChanged));
        }

        protected override void OnUnbind()
        {
            _singleDisposable.Dispose();
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void ColorChanged(Color color)
        {
            _text.color = color;
        }
    }
}