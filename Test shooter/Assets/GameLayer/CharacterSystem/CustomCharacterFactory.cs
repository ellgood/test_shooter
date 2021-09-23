using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;
using Zenject;

namespace GameLayer.CharacterSystem
{
    public sealed class CustomCharacterFactory : IFactory<GameObject,Vector3,ICharacter>
    {
        private const string ParentSlotKey = "[Character]";
        private readonly DiContainer _container;
        private GameObject _parent;

        public CustomCharacterFactory(DiContainer container)
        {
            _container = container;
        }
        public ICharacter Create(GameObject prefab, Vector3 position)
        {
            if (_parent == null)
            {
                _parent = new GameObject(ParentSlotKey);
                Object.DontDestroyOnLoad(_parent);
            }

            ICharacterComponent component =
                _container.InstantiatePrefabForComponent<CharacterComponent>(prefab, position,
                    Quaternion.identity, _parent.transform);
            return _container.Instantiate<Character>(new [] {component});
        }
    }
}