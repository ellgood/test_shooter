using System;
using CommonLayer.ResourceSystem.Config.Interfaces;

namespace CommonLayer.DataContext.Settings
{
    [Serializable]
    public sealed class CharacterSettings
    {
        public CharacterSettings(ICharacterConfig config)
        {
            Hash = config.GetHash();
            SetSensitivity(config.Sensitivity.Value);
            SetSpeed(config.Speed.Value);
            SetVerticalInverted(config.VerticalInverted);
        }

        public float Sensitivity { get; private set; }

        public float Speed { get; private set; }

        public bool VerticalInverted { get; private set; }

        public int Hash { get; }

        public void SetSensitivity(float value)
        {
            Sensitivity = value;
        }

        public void SetSpeed(float value)
        {
            Speed = value;
        }

        public void SetVerticalInverted(bool value)
        {
            VerticalInverted = value;
        }
    }
}