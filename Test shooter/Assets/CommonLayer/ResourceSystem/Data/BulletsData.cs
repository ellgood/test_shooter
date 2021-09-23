using System;
using CommonLayer.ResourceSystem.Config.Impl;
using CommonLayer.ResourceSystem.Config.Interfaces;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Data
{
    [Serializable]
    public sealed class BulletsData
    {
        [SerializeField]
        private BulletConfig simple;
        
        [SerializeField]
        private BulletConfig explosive;

        public IBulletConfig Simple => simple;

        public IBulletConfig Explosive => explosive;
    }
}