using System;
using System.Collections.Generic;
using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Impl;
using CommonLayer.ResourceSystem.Interface;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace GameLayer.GameParticleSystem
{
    public abstract class ParticleControllerBase : IParticleController, IInitializable, IDisposable, ITickable
    {
        protected abstract string ParentSlotKey { get; }
        private readonly ParticleFactory _particleFactory;

        private readonly Stack<IGameParticle> _nonActivePool = new Stack<IGameParticle>();
        private readonly Queue<IGameParticle> _activePool = new Queue<IGameParticle>();
        private readonly IResourcesController _resourcesController;
        private float _poolLifeTime;
        private int _minCount;
        private Transform _parent;
        private GameObject _particlePrefab;

        private float _timerTime;

        protected ParticleControllerBase(
            IResourcesController resourcesController,
            ParticleFactory particleFactory)
        {
            _resourcesController = resourcesController;
            _particleFactory = particleFactory;
        }

        protected abstract ParticleType ParticleType { get; }

        public void Dispose()
        {
            _nonActivePool.Clear();
            _activePool.Clear();
        }

        public void Initialize()
        {
            var particlesData = _resourcesController.GetData<ParticlesResourcesData>().Content;
            _particlePrefab = ParticleType == ParticleType.Muzzle ? particlesData.MuzzleFlash :
                ParticleType == ParticleType.SimpleHit ? particlesData.SimpleHit :
                particlesData.ExplodeHit;

            _poolLifeTime = particlesData.PoolLifeTime;
   
            _minCount = particlesData.MinPoolCount;
            _timerTime = 0;

            if (_parent != null) return;

            var obj = new GameObject(ParentSlotKey);
            _parent = obj.transform;
            Object.DontDestroyOnLoad(obj);
        }
        
        public void Play(Vector3 position,Vector3 normal)
        {
            var particle = Pick();
            _activePool.Enqueue(particle);
            particle.SetActive(true);
            var transform = particle.Transform;
            transform.position = position;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.LookRotation(normal);
            particle.ParticleSystem.Play();
        }
        private IGameParticle Pick()
        {
            if (_nonActivePool.Count > 0)
            {
                return _nonActivePool.Pop();
            }

            var particle = _particleFactory.Create(_particlePrefab, ParticleType, _parent);
            return particle;
        }
        
   

        public void Tick()
        {
            if (_activePool.Count > 0 && 
                !_activePool.Peek().ParticleSystem.IsAlive())
            {
                Return(_activePool.Dequeue());
            }
            
            
            if (_nonActivePool.Count <= _minCount) return;

            if (_timerTime <= _poolLifeTime)
            {
                _timerTime += Time.deltaTime;
            }
            else
            {
                while (_nonActivePool.Count > _minCount) Object.Destroy(_nonActivePool.Pop().Instance);

                _timerTime = 0;
            }
        }
        private void Return(IGameParticle particle)
        {
            particle.SetActive(false);
            _nonActivePool.Push(particle);
        }
     
    }
}