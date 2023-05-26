using System.Collections.Generic;
using DHAbilities.ResourceSystem;
using UnityEngine;

namespace DHAbilities.Samples.UI
{
    [DefaultExecutionOrder(1000)]
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIResourceManager : MonoBehaviour
    {
        [SerializeField]
        private ResourcePoolSystemComponent resourcePoolSystemComponent;

        [SerializeField]
        private GameObject uiResourcePoolPrefab;

        private void Awake()
        {
            foreach (KeyValuePair<string, ResourcePool> availableResourcePool in resourcePoolSystemComponent
                             .AvailableResourcePools)
                AddUIResourceComponent(availableResourcePool.Value);
            resourcePoolSystemComponent.OnResourcePoolAdded += AddUIResourceComponent;
        }

        private void OnDestroy() => resourcePoolSystemComponent.OnResourcePoolAdded -= AddUIResourceComponent;

        public void AddUIResourceComponent(ResourcePool resourcePool)
        {
            // spawn
            GameObject g = Instantiate(uiResourcePoolPrefab, transform);
            UIResourceComponent uiResourceComponent = g.GetComponent<UIResourceComponent>();

            // init
            uiResourceComponent.Init(resourcePool);
            g.SetActive(true);
        }
    }
}