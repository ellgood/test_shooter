using System;
using CommonLayer.DataContext.Settings;
using CommonLayer.ResourceSystem.Config.Data;
using CommonLayer.ResourceSystem.Config.Interfaces;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.DataBinding.Binds;
using CommonLayer.UserInterface.RouteSystem.Routes;
using CommonLayer.UserInterface.Utility;
using UniRx;

namespace CommonLayer.UserInterface.Presenter.Settings
{
    public abstract class SettingsPresenterBase : PresenterBase
    {
        private readonly ICharacterSettingsDataContext _characterSettingsCtx;
        private readonly ICharacterConfig _config;

        private readonly ReactiveProperty<bool> _invertedVerticalProp = new ReactiveProperty<bool>();

        private readonly ReactiveProperty<string> _invertedVerticalTextProp = new ReactiveProperty<string>();
        private readonly ReactiveProperty<float> _maxSensitivityProperty = new ReactiveProperty<float>();
        private readonly ReactiveProperty<float> _maxSpeedProperty = new ReactiveProperty<float>();

        private readonly ReactiveProperty<float> _minSensitivityProperty = new ReactiveProperty<float>();

        private readonly ReactiveProperty<float> _minSpeedProperty = new ReactiveProperty<float>();
        private readonly ReactiveProperty<string> _sensitivityTextProp = new ReactiveProperty<string>();
        private readonly ReactiveProperty<float> _sensitivityValueProp = new ReactiveProperty<float>();
        private readonly ReactiveProperty<string> _speedTextProp = new ReactiveProperty<string>();
        private readonly ReactiveProperty<float> _speedValueProp = new ReactiveProperty<float>();

        protected SettingsPresenterBase(
            ICharacterSettingsDataContext characterSettingsCtx)
        {
            _characterSettingsCtx = characterSettingsCtx;
            _config = _characterSettingsCtx.Config;
        }

        protected override void OnInit()
        {
            BindContext.Bind("sensitivity_value", _sensitivityValueProp,
                Observer.Create<float>(OnWriteSensitivityValue));
            BindContext.Bind("min_sensitivity_value", _minSensitivityProperty, RelationType.OneWay);
            BindContext.Bind("max_sensitivity_value", _maxSensitivityProperty, RelationType.OneWay);
            BindContext.Signal<float>("end_drag_sensitivity_signal", OnEndDragSensitivity);
            BindContext.TextData("sensitivity_text", _sensitivityTextProp);

            BindContext.Bind("speed_value", _speedValueProp,
                Observer.Create<float>(OnWriteSpeedValue));
            BindContext.Bind("min_speed_value", _minSpeedProperty, RelationType.OneWay);
            BindContext.Bind("max_speed_value", _maxSpeedProperty, RelationType.OneWay);
            BindContext.Signal<float>("end_drag_speed_signal", OnEndDragSpeed);
            BindContext.TextData("speed_text", _speedTextProp);


            BindContext.Bind("inverted_vertical_toggle", _invertedVerticalProp,
                Observer.Create<bool>(OnInvertedVerticalChanged));
            BindContext.TextData("inverted_vertical_text", _invertedVerticalTextProp);

            BindContext.Signal("back_button", OnBackButton);
            BindContext.Signal("menu_button", OnMenuButton);
        }

        protected abstract void OnMenuButton();

        protected override void OnShow(Action onShowed)
        {
            base.OnShow(onShowed);

            _minSensitivityProperty.Value = 0;
            _maxSensitivityProperty.Value = 1;
            _minSpeedProperty.Value = 0;
            _maxSpeedProperty.Value = 1;

            var state = _characterSettingsCtx.VerticalInverted;
            SetInvertedVertical(state);
            FillSliderValue(_speedTextProp, _speedValueProp, _characterSettingsCtx.Speed, _config.Speed);
            FillSliderValue(_sensitivityTextProp, _sensitivityValueProp, _characterSettingsCtx.Sensitivity,
                _config.Sensitivity);
        }

        private void FillSliderValue(
            ReactiveProperty<string> textProp,
            ReactiveProperty<float> sliderProp,
            float value,
            FloatRestraintParam restraint)
        {
            textProp.Value = $"{value:F2}";
            sliderProp.Value = MathUtils.Map01(value, restraint.MinValue, restraint.MaxValue);
        }

        protected override void OnHide(Action onHidden)
        {
            base.OnHide(onHidden);
            _characterSettingsCtx.SaveSettings();
        }

        private void OnEndDragSpeed(float value)
        {
            var mapValue = MapSliderValue(value, _config.Speed);
            _speedTextProp.Value = $"{mapValue:F2}";
            _characterSettingsCtx.SetSpeed(mapValue);
        }

        private void OnEndDragSensitivity(float value)
        {
            var mapValue = MapSliderValue(value, _config.Sensitivity);
            _sensitivityTextProp.Value = $"{mapValue:F2}";
            _characterSettingsCtx.SetSensitivity(mapValue);
        }

        private void OnWriteSpeedValue(float value)
        {
            var mapValue = MapSliderValue(value, _config.Speed);
            _speedTextProp.Value = $"{mapValue:F2}";
        }

        private void OnWriteSensitivityValue(float value)
        {
            var mapValue = MapSliderValue(value, _config.Sensitivity);
            _sensitivityTextProp.Value = $"{mapValue:F2}";
        }

        private void OnInvertedVerticalChanged(bool state)
        {
            SetInvertedVertical(state);
            _characterSettingsCtx.SetVerticalInverted(state);
        }

        private float MapSliderValue(float value, FloatRestraintParam restraint)
        {
            return MathUtils.Map(value, 0, 1, restraint.MinValue, restraint.MaxValue);
        }

        private void SetInvertedVertical(bool state)
        {
            _invertedVerticalProp.Value = state;
            _invertedVerticalTextProp.Value = state ? "On" : "Off";
        }

        private void OnBackButton()
        {
            Route.RouteHide();
        }
    }
}