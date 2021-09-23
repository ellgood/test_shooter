using CommonLayer.DataContext.Settings;

namespace GameLayer.CharacterSystem.CharacterMovementSystem
{
    public interface ICharacterMovementControl : IGameCharacterSubControl<ICharacterMoveSettingsInfo>
    {
      bool SitDownFlag { get; }
    }
}