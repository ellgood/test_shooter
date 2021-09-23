namespace GameLayer.WeaponSystem
{
    public interface IWeaponSettings
    {
        float Damage { get; }
        float Range { get; }
        float FireRate { get; }
        float ImpactForce { get; }
    }
}