using CommonLayer.DataContext.Settings;

namespace GameLayer.CharacterSystem
{
    public interface IGameCharacterSubControl <in TSettings>
    where TSettings : ICharacterSettingsInfo
    {
        void Init(ICharacterComponent characterComponent, TSettings settingsInfo);
        void Update();
    }
}