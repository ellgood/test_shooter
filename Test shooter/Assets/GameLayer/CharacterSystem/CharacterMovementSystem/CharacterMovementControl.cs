using CommonLayer.DataContext.Settings;
using UnityEngine;

namespace GameLayer.CharacterSystem.CharacterMovementSystem
{
    public sealed class CharacterMovementControl : ICharacterMovementControl
    {
        private const string HorAxisKey = "Horizontal";
        private const string VertAxisKey = "Vertical";
        private ICharacterComponent _component;


        private Vector2 _currentDir;
        private Vector2 _currentDirVelocity;

        private float _lerpTime;
        private ICharacterMoveSettingsInfo _settings;

        private float _velocityY;
        
        public bool SitDownFlag { get; private set; }

        public void Init(ICharacterComponent component, ICharacterMoveSettingsInfo settings)
        {
            _component = component;
            _settings = settings;

            _currentDir = Vector2.zero;
            _currentDirVelocity = Vector2.zero;
            _velocityY = 0;
            _lerpTime = 0;
        }

        public void Update()
        {
            var targetDir = new Vector2(Input.GetAxisRaw(HorAxisKey), Input.GetAxisRaw(VertAxisKey));
            targetDir.Normalize();

            _currentDir = Vector2.SmoothDamp(_currentDir, targetDir, ref _currentDirVelocity,
                _settings.MovementSmoothFactor);

            var controller = _component.Controller;
            var transform = _component.CharacterSlotTransform;
            _velocityY += _settings.Gravity * Time.deltaTime;
            var speed = _settings.Speed;
            SitDownFlag = false;
            if (controller.isGrounded)
            {
                controller.stepOffset = _settings.StepOffSet;
                _velocityY = _settings.GroundedVelocityY;

                if (Input.GetKeyDown(KeyCode.Space))
                    _velocityY = Mathf.Sqrt(_settings.JumpHeight * -controller.height * _settings.Gravity);

                var lastBodyPosition = _component.BodySlotTransform.localPosition;
                var yPoint = lastBodyPosition.y;

                var lerpFactor = _settings.MovementSmoothFactor;

                if (_lerpTime >= 1)
                {
                    lerpFactor = 1;
                    _lerpTime = 0;
                }

                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    yPoint = Mathf.SmoothStep(lastBodyPosition.y, -_settings.SitPointOffsetY, lerpFactor);
                    _lerpTime += Time.deltaTime;
                    _component.BodySlotTransform.localPosition =
                        new Vector3(lastBodyPosition.x, yPoint, lastBodyPosition.z);
                    speed *= _settings.SitSpeedFactor;
                    SitDownFlag = true;
                }
                else if (yPoint < 0)
                {
                    yPoint = Mathf.SmoothStep(lastBodyPosition.y, 0, lerpFactor);
                    _lerpTime += Time.deltaTime;
                    _component.BodySlotTransform.localPosition =
                        new Vector3(lastBodyPosition.x, yPoint, lastBodyPosition.z);
                }
            }
            else
            {
                controller.stepOffset = 0;
            }

            var velocity = (transform.forward * _currentDir.y + transform.right * _currentDir.x) * speed +
                           Vector3.up * _velocityY;
            controller.Move(velocity * Time.deltaTime);
        }

   
    }
}