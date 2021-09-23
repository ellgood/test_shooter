using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CommonLayer.UserInterface.Binder.Image
{
    [RequireComponent(typeof(RawImage))]
    public sealed class UiRawImageBinder : ViewBinderBase
    {
        [SerializeField]
        private string _textureKey;

        [SerializeField]
        private string _colorKey;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private RawImage _image;

        public void TextureChanged(Texture2D texture)
        {
            _image.texture = texture;
        }

        public void ColorChanged(Color color)
        {
            _image.color = color;
        }

        protected override void OnBind(BindContext bindContext)
        {
            bindContext.Bind(_textureKey, Observer.Create<Texture2D>(TextureChanged)).AddTo(_disposable);

            if (!string.IsNullOrEmpty(_colorKey))
            {
                bindContext.Bind(_colorKey, Observer.Create<Color>(ColorChanged)).AddTo(_disposable);
            }
        }

        protected override void OnUnbind()
        {
            _disposable.Clear();
        }

        private void Awake()
        {
            _image = GetComponent<RawImage>();
        }
    }
}