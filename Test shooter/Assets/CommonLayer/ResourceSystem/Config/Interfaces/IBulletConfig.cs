
using CommonLayer.ResourceSystem.Data;

namespace CommonLayer.ResourceSystem.Config.Interfaces
{
    public interface IBulletConfig : IConfig
    {
        float Damage { get; }

        float Force { get; }

        float ExplosionRadius { get; }

        BulletType Type { get; }
    }
}