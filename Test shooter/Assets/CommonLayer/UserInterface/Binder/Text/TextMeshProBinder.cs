using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.Objects;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommonLayer.UserInterface.Binder.Text
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class TextMeshProBinder : ViewBinderBase
    {
        private const string KeyExGroup = "key";
        private const string FormatExGroup = "frm";

        private const string PatternEx = @"(?<={)(?'" + KeyExGroup + @"'\w+)(?>:(?'" + FormatExGroup +
                                         @"'[\\\-\.\w#,+‰%':" +
                                         "\\\"" +
                                         @"; ]+))?(?=})";

        private static readonly List<ParseInfo> InfoTempCache = new List<ParseInfo>();

        [SerializeField]
        private bool _useLateUpdate;

        [SerializeField]
        [MultilineReactiveProperty]
        private StringReactiveProperty _formatText;

        [SerializeField]
        private string _textColorKey;

        [SerializeField]
        private float _tweenDuration;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private readonly Dictionary<int, TextInternalBinder> _internalBinder =
            new Dictionary<int, TextInternalBinder>();

        private readonly ScrambleMode _scrambleTweenMode = ScrambleMode.Uppercase;

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private BindContext _bindCtx;

        private IDisposable _formatChange;
        private bool _isBound;

        private bool _isDirty;

        private ParseInfo[] _parseInfos;

        private TextMeshProUGUI _text;

        public event Action<string> TextChanged;

        public string FormatText
        {
            get => _formatText.Value;
            set => _formatText.Value = value;
        }

        public TextMeshProUGUI TextMeshPro => _text ? _text : _text = GetComponent<TextMeshProUGUI>();

        protected override void OnEnable()
        {
            base.OnEnable();

            _formatChange = _formatText.Subscribe(OnFormatTextChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _formatChange.Dispose();
            DoUnbind();
        }

        protected override void OnBind(BindContext bindContext)
        {
            _bindCtx = bindContext;

            DoBind();
        }

        protected override void OnUnbind()
        {
            DoUnbind();
            _bindCtx = null;
        }

        private void LateUpdate()
        {
            if (_useLateUpdate)
            {
                TextRefresh();
            }
        }

        private void OnFormatTextChange(string text)
        {
            _parseInfos = Parse(text);

            DoUnbind();
            DoBind();
        }

        private void DoBind()
        {
            if (_isBound || _bindCtx == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(_textColorKey))
            {
                _bindCtx.Bind(_textColorKey, Observer.Create<Color>(c => TextMeshPro.color = c)).AddTo(_disposable);
            }

            if (_parseInfos != null)
            {
                foreach (ParseInfo parseInfo in _parseInfos)
                {
                    if (parseInfo.IsKey)
                    {
                        _internalBinder.Add(parseInfo.Position,
                            new TextInternalBinder(parseInfo.Value, parseInfo.Format, _bindCtx, Repaint).AddTo(_disposable));
                    }
                }
            }

            foreach (TextInternalBinder internalBinderValue in _internalBinder.Values)
            {
                internalBinderValue.SetActive(true);
            }

            Repaint();
            _isBound = true;
        }

        private void Repaint()
        {
            _isDirty = true;
            if (!_useLateUpdate && isActiveAndEnabled)
            {
                TextRefresh();
            }
        }

        private void DoUnbind()
        {
            if (!_isBound)
            {
                return;
            }

            _disposable.Clear();
            _internalBinder.Clear();

            _isBound = false;
        }

        private static ParseInfo[] Parse(string formattedText)
        {
            Match match = Regex.Match(formattedText, PatternEx);

            try
            {
                for (var i = 0; i < formattedText.Length;)
                {
                    if (match.Success)
                    {
                        int matchIndex = match.Index;

                        if (matchIndex - 1 == i)
                        {
                            Group keyGroup = match.Groups[KeyExGroup];
                            Group formatGroup = match.Groups[FormatExGroup];

                            bool hasKey = keyGroup.Success;
                            bool hasFormat = formatGroup.Success;

                            if (hasKey && hasFormat)
                            {
                                InfoTempCache.Add(new ParseInfo(true, keyGroup.Value, formatGroup.Value, matchIndex, match.Length));
                            }
                            else
                            {
                                InfoTempCache.Add(new ParseInfo(true, keyGroup.Value, string.Empty, matchIndex, match.Length));
                            }

                            //Ignore brackets '{_}'
                            i = matchIndex + match.Length + 1;
                            match = match.NextMatch();
                        }
                        else
                        {
                            int len = matchIndex - i - 1;
                            InfoTempCache.Add(new ParseInfo(false, formattedText.Substring(i, len), string.Empty, i, len));
                            i += len;
                        }
                    }
                    else
                    {
                        int textLength = formattedText.Length - i;

                        InfoTempCache.Add(new ParseInfo(false, formattedText.Substring(i, textLength), string.Empty, i, textLength));
                        i = formattedText.Length;
                    }
                }

                return InfoTempCache.ToArray();
            }
            finally
            {
                InfoTempCache.Clear();
            }
        }

        private void TextRefresh()
        {
            if (!_isDirty)
            {
                return;
            }

            _stringBuilder.Clear();

            foreach (ParseInfo parseInfo in _parseInfos)
            {
                if (parseInfo.IsKey)
                {
                    if (_internalBinder.TryGetValue(parseInfo.Position, out TextInternalBinder binder))
                    {
                        binder.Append(_stringBuilder);
                    }
                    else
                    {
                        _stringBuilder.Append($"{{{parseInfo.Value}}}");
                    }
                }
                else
                {
                    _stringBuilder.Append(parseInfo.Value);
                }
            }

            if (_tweenDuration > 0)
            {
                var endValue = _stringBuilder.ToString();
                TweenerCore<string, string, StringOptions> t = DOTween.To(() => TextMeshPro.text, x => TextMeshPro.SetText(x), endValue, _tweenDuration);
                t.SetOptions(TextMeshPro.richText, _scrambleTweenMode).SetTarget(TextMeshPro);
            }
            else
            {
                TextMeshPro.SetText(_stringBuilder);
            }

            TextChanged?.Invoke(_stringBuilder.ToString());

            _isDirty = false;
        }

        private sealed class TextInternalBinder : DisposableObject
        {
            private const string NullRefValue = @"$NullRef$";
            private const string InvalidFormatValue = @"$InvFrm$";
            private const string DefaultValue = "$NoDef$";

            private readonly Action _change;

            private readonly IDisposable _valueBind;
            private readonly string _format;

            private string _cachedValue;

            private bool _active;

            public TextInternalBinder(string key, string format, BindContext bindContext, Action change)
            {
                Assert.IsNotNull(bindContext);
                Assert.IsNotNull(change);

                _change = change;
                _format = format;

                _cachedValue = DefaultValue;
                _valueBind = bindContext.Bind(key, Observer.Create<IFormattable>(OnValueChanged));
            }

            public void SetActive(bool active)
            {
                _active = active;
            }

            public void Append(StringBuilder stringBuilder)
            {
                ThrowIfDisposedDebugOnly();

                stringBuilder.Append(_cachedValue);
            }

            protected override void OnDispose()
            {
                _valueBind?.Dispose();
            }

            private void OnValueChanged(IFormattable formattable)
            {
                ThrowIfDisposedDebugOnly();

                try
                {
                    _cachedValue = formattable.ToString(_format, CultureInfo.CurrentCulture);
                }
                catch (NullReferenceException)
                {
                    _cachedValue = NullRefValue;
                }
                catch (FormatException)
                {
                    _cachedValue = InvalidFormatValue;
                }

                if (_active)
                {
                    _change.Invoke();
                }
            }
        }

        private struct ParseInfo
        {
            public ParseInfo(
                bool isKey,
                string value,
                string format,
                int position,
                int length)
            {
                IsKey = isKey;
                Value = value;
                Format = format;
                Position = position;
                Length = length;
            }

            public int Position { get; }
            public int Length { get; }

            public string Value { get; }

            public string Format { get; }

            public bool IsKey { get; }
        }
    }
}