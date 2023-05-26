using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DHAbilities.ResourceSystem
{
    public class ResourcePoolSystemComponent : MonoBehaviour
    {
        public event Action OnUpdate;
        // TODO: add and remove
        public event Action<ResourcePool> OnResourcePoolAdded;
        public event Action<ResourcePool> OnResourcePoolRemoved;
        
        public readonly Dictionary<string, ResourcePool> AvailableResourcePools = new Dictionary<string, ResourcePool>();

        [Tooltip("specify all pools that shall be allocated for this system by providing the base data object")]
        public ResourcePoolBaseData[] resourcePoolsBaseData;

        private void Awake()
        {
            foreach (var resourcePoolBaseData in resourcePoolsBaseData)
                AvailableResourcePools.Add(resourcePoolBaseData.Name, new ResourcePool(this, resourcePoolBaseData));
            resourcePoolsBaseData = null; // cleanup
        }

        private void Update()
            => OnUpdate?.Invoke();

        /// <summary>
        /// Checks and deducts in one go. Resources are only deducted if all resource requirements are satisfied.
        /// </summary>
        /// <param name="requiredResources"></param>
        /// <returns>Weather all required resources were deducted</returns>
        public bool TryDeductResources(Dictionary<string, float> requiredResources)
        {
            if (!CheckResourcesAvailable(requiredResources))
                return false;

            DeductResources(requiredResources);
            return true;
        }

        public bool CheckResourcesAvailable(Dictionary<string, float> requiredResources)
            => requiredResources.All(x => CheckResourceAvailable(x.Key, x.Value));

        public bool CheckResourceAvailable(string type, float amount)
            => AvailableResourcePools[type].AvailableResource >= amount;

        public void DeductResources(Dictionary<string, float> resourcesToDeduct)
        {
            foreach (KeyValuePair<string, float> x in resourcesToDeduct)
                DeductResource(x.Key, x.Value);
        }

        public void DeductResource(string type, float amount)
            => AvailableResourcePools[type].RemoveResource(amount);

        public void AddResources(Dictionary<string, float> resourcesToAdd)
        {
            foreach (KeyValuePair<string, float> x in resourcesToAdd)
                AddResource(x.Key, x.Value);
        }

        public void AddResource(string type, float amount)
            => AvailableResourcePools[type].AddResource(amount);
    }
}