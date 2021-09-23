using CommonLayer.ResourceSystem.Config.Interfaces;

namespace CommonLayer.ResourceSystem.Config
{
    public abstract class ConfigBase :IConfig
    {
        public abstract int GetHash();
    }
}