using DHAbilities.ResourceSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DHAbilities.Samples.UI
{
    public sealed class UIResourceComponent : MonoBehaviour
    {
        private ResourcePool _resourcePool;
        [SerializeField] private RectTransform barFillingTransform;
        [SerializeField] private Image barFillingImage;
        [SerializeField] private Text nameLabel;
        [SerializeField] private int statsTextFloatPrecision;
        [SerializeField] private Text statsText;

        private void OnEnable()
        {
            _resourcePool.OnAvailableResourceChanged += UpdateBarAndNumbers;
            _resourcePool.OnMaxResourceChanged += UpdateBarAndNumbers;
        }

        private void OnDisable()
        {
            _resourcePool.OnAvailableResourceChanged -= UpdateBarAndNumbers;
            _resourcePool.OnMaxResourceChanged -= UpdateBarAndNumbers;
        }

        public void Init(ResourcePool resourcePool)
        {
            _resourcePool = resourcePool;
            var baseData = _resourcePool.ResourcePoolBaseData;
            barFillingImage.color = baseData.UIBarColor;
            nameLabel.text = baseData.UIName;
            UpdateBarAndNumbers(0f);
        }

        private void UpdateBarAndNumbers(float _)
        {
            barFillingTransform.localScale =
                    new Vector3(1, _resourcePool.AvailableResource / _resourcePool.MaxResource, 1);
            statsText.text = string.Format("{0:F" + statsTextFloatPrecision + "} / {1:F" + statsTextFloatPrecision + "}", _resourcePool.AvailableResource, _resourcePool.MaxResource);
        }
    }
}