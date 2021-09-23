using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommonLayer.UserInterface.Binder.Image
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public sealed class UiImageColorBinder : ViewBinderBase
    {
        private const float MixedColorCoef = 0.5f;

        [SerializeField]
        private string _colorKey;

        [SerializeField]
        private ColorMixingMode _mixing;

        [Tooltip("Works only in DisableMixing mode.")]
        [SerializeField]
        private ColorMode _colorMode = ColorMode.RGBA;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private Color _cachedColor;

        private UnityEngine.UI.Image _image;

        protected override void OnBind(BindContext bindContext)
        {
            _image = GetComponent<UnityEngine.UI.Image>();
            Assert.IsNotNull(_image);

            _cachedColor = _image.color;

            if (string.IsNullOrWhiteSpace(_colorKey))
            {
                throw new Exception("Incorrect color binder key.");
            }

            bindContext.Bind(_colorKey, Observer.Create<Color>(ColorChanged)).AddTo(_disposable);
        }

        protected override void OnUnbind()
        {
            _image = null;
            _cachedColor = Color.white;

            _disposable.Clear();
            if (_image != null)
            {
                _image.color = _cachedColor;
            }
        }

        private void ColorChanged(Color color)
        {
            if (!enabled)
            {
                return;
            }

            switch (_mixing)
            {
                case ColorMixingMode.MixWithCurrent:
                {
                    color = (_image.color + color) * MixedColorCoef;
                    break;
                }
                case ColorMixingMode.MixWithBuffered:
                {
                    color = (_cachedColor + color) * MixedColorCoef;
                    break;
                }
                case ColorMixingMode.DisableMixing:
                {
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }

            if (_mixing == ColorMixingMode.DisableMixing)
            {
                ApplyColorByMode(color);
            }
            else
            {
                _image.color = color;
            }
        }

        private void ApplyColorByMode(Color color)
        {
            Color oldColor = _image.color;
            float r = (_colorMode & ColorMode.R) == ColorMode.R ? color.r : oldColor.r;
            float g = (_colorMode & ColorMode.G) == ColorMode.G ? color.g : oldColor.g;
            float b = (_colorMode & ColorMode.B) == ColorMode.B ? color.b : oldColor.b;
            float a = (_colorMode & ColorMode.A) == ColorMode.A ? color.a : oldColor.a;
            _image.color = new Color(r, g, b, a);
        }
        
        private enum ColorMixingMode
        {
            DisableMixing,
            MixWithCurrent,
            MixWithBuffered
        }

        [Flags]
        private enum ColorMode : byte
        {
            None = 0,
            R = 1,
            G = 2,
            B = 4,
            A = 8,
            RG = R | G,
            RB = R | B,
            RA = R | A,
            GB = G | B,
            GA = G | A,
            BA = B | A,
            RGB = R | G |B,
            RGBA = R | G | B | A
        }
    }
}