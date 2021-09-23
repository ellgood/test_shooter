using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CommonLayer.ResourceSystem.Interface
{
    public sealed class ResourcesController : IResourcesController
    {
        private readonly Dictionary<Type, List<IResourceData>> _dataDictionary =
            new Dictionary<Type, List<IResourceData>>();

        private bool _isLoaded;

        private bool _isLoading;

        public ResourcesController()
        {
            PreLoadAllData();
        }

        private void PreLoadAllData()
        {
            if (_isLoading || _isLoaded) return;

            _isLoading = true;

            try
            {
                var loadedData = Resources.LoadAll("DataBases/", typeof(IResourceData));
                foreach (var o in loadedData) RegisterTo((IResourceData)o, o.GetType());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return;
            }

            _isLoading = false;
            _isLoaded = true;
        }

        private void RegisterTo(IResourceData resourceData, Type type)
        {
            while (type != null)
            {
                if (!_dataDictionary.TryGetValue(type, out var values))
                {
                    values = new List<IResourceData> { resourceData };
                    _dataDictionary[type] = values;
                }
                else
                {
                    values.Add(resourceData);
                }

                type = type.BaseType;
            }
        }

        #region IResourcesDataContext Implementation

        public T GetData<T>()
            where T : Object, IResourceData
        {
            return TryGetData(out T value) ? value : throw new KeyNotFoundException("Not found data type");
        }

        public bool TryGetData<T>(out T value)
            where T : Object, IResourceData
        {
            var result = GetDataSet<T>();
            if (result.Length == 1)
            {
                value = result[0];
                return true;
            }

            if (result.Length > 1)
                throw new ArgumentException($"Result not one try use {nameof(GetDataSet)} method for this data type");

            value = default;
            return false;
        }

        public T[] GetDataSet<T>()
            where T : Object, IResourceData
        {
            var dataType = typeof(T);

            return _dataDictionary.TryGetValue(dataType, out var data)
                ? data.Cast<T>().ToArray()
                : Array.Empty<T>();
        }

        #endregion
    }
}