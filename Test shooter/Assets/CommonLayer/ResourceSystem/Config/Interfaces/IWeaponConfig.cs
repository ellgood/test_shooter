namespace CommonLayer.ResourceSystem.Config.Interfaces
{
    public interface IWeaponConfig : IConfig
    {
        float Range { get; }
        float FireRate { get; }
        float DamageMtp { get; }
    }
}