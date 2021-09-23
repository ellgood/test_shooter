using CommonLayer.ResourceSystem.Config.Interfaces;

namespace CommonLayer.DataContext.Settings
{
    public interface ICharacterSettingsDataContext : IDataContext,ICharacterLookSettingsInfo, ICharacterMoveSettingsInfo
    {
        ICharacterConfig Config { get; }
        void SetSensitivity(float value);
        void SetSpeed(float value);
        void SetVerticalInverted(bool value);
        void SaveSettings();
        void LoadSettings();
    }
}