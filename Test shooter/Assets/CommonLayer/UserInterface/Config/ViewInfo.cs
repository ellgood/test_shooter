using System;
using CommonLayer.UserInterface.Views;
using UnityEngine;

namespace CommonLayer.UserInterface.Config
{
    [Serializable]
    public class ViewInfo
    {
        [SerializeField]
        private string _viewKey;
        
        [SerializeField]
        private ViewBase _view;

        [SerializeField]
        private int _layer;

        [SerializeField]
        private float _poolLifeTime;

        [SerializeField]
        private int _maxDefaultPoolCount;

        [SerializeField]
        private bool _useSafeZone = true;

        public ViewBase View => _view;
        public int Layer => _layer;
        public float PoolLifeTime => _poolLifeTime;
        public int MaxDefaultPoolCount => _maxDefaultPoolCount;
        public bool UseSafeZone => _useSafeZone;

        public string ViewKey => _viewKey;
    }
}