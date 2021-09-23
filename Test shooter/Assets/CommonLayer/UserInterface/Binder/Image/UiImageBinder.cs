using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Image
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public sealed class UiImageBinder : ViewBinderBase
    {
        [SerializeField]
        private string _spriteKey;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private UnityEngine.UI.Image _image;

        protected override void OnBind(BindContext bindContext)
        {
            if (string.IsNullOrWhiteSpace(_spriteKey))
            {
                throw new Exception("Incorrect color binder key.");
            }

            bindContext.Bind(_spriteKey, Observer.Create<Sprite>(SpriteChanged)).AddTo(_disposable);
        }

        protected override void OnUnbind()
        {
            _disposable.Clear();
            _image.sprite = null;
        }

        private void Awake()
        {
            _image = GetComponent<UnityEngine.UI.Image>();
        }

        private void SpriteChanged(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}