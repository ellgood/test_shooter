using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace CommonLayer.UserInterface.Binder.Text
{
    [RequireComponent(typeof(TextMeshProBinder))]
    public sealed class TextMeshProBinderEvent : MonoBehaviour
    {
        [SerializeField]
        private StringEvent TextChanged;

        private TextMeshProBinder _tmpBinder;

        public void AddListener(UnityAction<string> call)
        {
            TextChanged.AddListener(call);
        }

        public void RemoveListener(UnityAction<string> call)
        {
            TextChanged.RemoveListener(call);
        }

        private void Awake()
        {
            _tmpBinder = GetComponent<TextMeshProBinder>();

            Assert.IsNotNull(_tmpBinder);
        }

        private void OnEnable()
        {
            _tmpBinder.TextChanged += OnTextChanged;
        }

        private void OnDisable()
        {
            _tmpBinder.TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(string text)
        {
            TextChanged.Invoke(text);
        }

        [Serializable]
        public sealed class StringEvent : UnityEvent<string> { }
    }
}