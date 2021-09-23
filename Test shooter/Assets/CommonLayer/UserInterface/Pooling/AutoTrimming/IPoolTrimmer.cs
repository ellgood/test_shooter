using System;

namespace CommonLayer.UserInterface.Pooling.AutoTrimming
{
    public interface IPoolTrimmer : IDisposable
    {
        float TrimDelay { get; set; }
        float TrimRatio { get; set; }
        int MinSize { get; set; }
        bool CallOnBeforeRent { get; set; }

        void OnReturn();

        void Check();
    }
}