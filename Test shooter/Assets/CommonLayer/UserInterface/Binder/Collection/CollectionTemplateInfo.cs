using System;
using CommonLayer.UserInterface.Views;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Collection
{
    [Serializable]
    public struct CollectionTemplateInfo
    {
        [SerializeField]
        private ViewElement _template;

        [SerializeField]
        private Transform _templatePool;

        [SerializeField]
        private Transform _templateContentPanel;

        public ViewElement Template => _template;

        public Transform TemplatePool => _templatePool;

        public Transform TemplateContentPanel => _templateContentPanel;
    }
}