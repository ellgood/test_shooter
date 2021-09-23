namespace CommonLayer.DataContext.Settings
{
    public interface ICharacterMoveSettingsInfo : ICharacterSettingsInfo
    {
        float Speed { get; }
        float MovementSmoothFactor { get;}
        float Gravity { get;}
        float StepOffSet { get;}
        float GroundedVelocityY { get;}
        float JumpHeight { get;}
        float SitPointOffsetY { get;}
        float SitSpeedFactor { get;}
    }
}