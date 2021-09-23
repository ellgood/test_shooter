using CommonLayer.ResourceSystem.Config.Data;

namespace CommonLayer.ResourceSystem.Config.Interfaces
{
    public interface IEnemyConfig : IConfig
    {
        IntRestraintParam Health { get; }
    }
}