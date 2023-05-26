using System;
using UnityEngine;

namespace DHAbilities.ResourceSystem
{
    public sealed class ResourcePool
    {
        public readonly ResourcePoolBaseData ResourcePoolBaseData;

        public event Action<float> OnAvailableResourceChanged;
        public event Action<float> OnMaxResourceChanged;

        public readonly string name;

        public float AvailableResource
        {
            get => _availableResource;
            private set
            {
                _availableResource = value;
                OnAvailableResourceChanged?.Invoke(value);
            }
        }

        public float MaxResource
        {
            get => _maxResource;
            set
            {
                _maxResource = value;
                OnMaxResourceChanged?.Invoke(value);
            }
        }

        public float PassiveRegeneration;
        private float _maxResource;

        private bool _regenerating;
        private float _availableResource;
        private readonly ResourcePoolSystemComponent _owningSystem;


        public ResourcePool(ResourcePoolSystemComponent owningSystem, ResourcePoolBaseData resourcePoolBaseData)
        {
            _owningSystem = owningSystem;
            owningSystem.OnUpdate += ApplyPassiveRegeneration;
            _regenerating = true;
            ResourcePoolBaseData = resourcePoolBaseData;

            // init default values
            name = resourcePoolBaseData.name;
            AvailableResource = resourcePoolBaseData.StartingResource;
            MaxResource = resourcePoolBaseData.MaxResource;
            PassiveRegeneration = resourcePoolBaseData.PassiveRegeneration;
        }

        ~ResourcePool()
        {
            if (_regenerating)
                _owningSystem.OnUpdate -= ApplyPassiveRegeneration;
        }


        private void ApplyPassiveRegeneration()
        {
            if (_regenerating && Mathf.Approximately(AvailableResource, MaxResource))
            {
                _owningSystem.OnUpdate -= ApplyPassiveRegeneration;
                _regenerating = false;
                return;
            }

            AvailableResource = Mathf.Min(MaxResource, AvailableResource + PassiveRegeneration * Time.deltaTime);
        }

        /// <remarks>Values are clamped at maximum</remarks>
        public void AddResource(float amountToAdd) =>
                AvailableResource = Mathf.Min(MaxResource, AvailableResource + amountToAdd);

        /// <remarks>Values are clamped at 0</remarks>
        public void RemoveResource(float amountToSubtract)
        {
            AvailableResource = Mathf.Max(0, AvailableResource - amountToSubtract);
            if (!_regenerating)
            {
                _owningSystem.OnUpdate += ApplyPassiveRegeneration;
                _regenerating = true;
            }
        }
    }
}