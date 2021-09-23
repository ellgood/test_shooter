using System;

namespace CommonLayer.DataContext.Settings
{
    public interface ICharacterSettingsInfo
    {
        event Action<CharacterSettingsType> SettingsChanged;
    }
}