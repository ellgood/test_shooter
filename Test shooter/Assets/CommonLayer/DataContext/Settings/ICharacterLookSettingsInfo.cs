namespace CommonLayer.DataContext.Settings
{
    public interface ICharacterLookSettingsInfo : ICharacterSettingsInfo
    {
        float Sensitivity { get; }
        bool VerticalInverted { get; }
        float LookSmoothFactor { get; }
        float VerticalRestraint { get; }
        bool CursorVisible { get;}
    }
}