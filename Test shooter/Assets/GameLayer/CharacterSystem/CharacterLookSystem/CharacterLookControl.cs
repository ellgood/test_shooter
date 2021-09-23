using CommonLayer.DataContext.Settings;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameLayer.CharacterSystem.CharacterLookSystem
{
    public sealed class CharacterLookControl : ICharacterLookControl
    {
        private const string MouseXKey = "Mouse X";
        private const string MouseYKey = "Mouse Y";

        private float _cameraPitch;
        private ICharacterComponent _component;

        private Vector2 _currentMouseDelta;
        private Vector2 _currentMouseDeltaVelocity;
    
        private ICharacterLookSettingsInfo _settings;

        public void Init(ICharacterComponent component, ICharacterLookSettingsInfo settings)
        {
            _component = component;
            _settings = settings;
            
            _cameraPitch = 0;
       
            _currentMouseDelta = Vector2.zero;
            _currentMouseDeltaVelocity = Vector2.zero;
        }

        public void Update()
        {
            Assert.IsNotNull(_component);
            Assert.IsNotNull(_settings);

            var invertFactor = _settings.VerticalInverted ? -1 : 1;
            var targetMouseDelta = new Vector2(Input.GetAxis(MouseXKey), Input.GetAxis(MouseYKey) * invertFactor);

            _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetMouseDelta,
                ref _currentMouseDeltaVelocity, _settings.LookSmoothFactor);

            _cameraPitch -= _currentMouseDelta.y * _settings.Sensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -_settings.VerticalRestraint,
                _settings.VerticalRestraint);

            _component.LookSlotTransform.localEulerAngles = Vector3.right * _cameraPitch;
            _component.CharacterSlotTransform.Rotate(Vector3.up * (_currentMouseDelta.x * _settings.Sensitivity));
        }
    }
}