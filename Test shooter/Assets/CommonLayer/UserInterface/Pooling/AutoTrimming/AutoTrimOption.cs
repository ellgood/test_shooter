using UnityEngine;

namespace CommonLayer.UserInterface.Pooling.AutoTrimming
{
    public struct AutoTrimOption
    {
        private float _trimDelay;
        private float _trimRatio;
        private int _minSize;

        public float TrimDelay
        {
            get => _trimDelay;
            set => _trimDelay = Mathf.Clamp(value, 0, float.MaxValue);
        }

        /// <summary>
        /// 0.0f = clear 100% 1.0f = don't clear anything.
        /// </summary>
        public float TrimRatio
        {
            get => _trimRatio;
            set => _trimRatio = Mathf.Clamp01(value);
        }

        public int MinSize
        {
            get => _minSize;
            set => _minSize = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public bool CallOnBeforeRent { get; set; }

        public bool Enabled { get; set; }
    }
}