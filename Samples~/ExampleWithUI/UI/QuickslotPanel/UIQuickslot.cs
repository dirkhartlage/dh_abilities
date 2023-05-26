using DHAbilities.AbilitySystem;
using UnityEngine;
using UnityEngine.UI;

namespace DHAbilities.Samples.UI.QuickslotPanel
{
    public sealed class UIQuickslot : MonoBehaviour
    {
        [SerializeField]
        private Text nameLabel;

        [SerializeField]
        private Image thumbnail;

        [SerializeField]
        private Image cooldownIndicator;

        [SerializeField]
        private Text cooldownText;

        private Ability _ability;
        private Sprite _defaultThumbnailSprite;
        private bool _coolingDown;
        private Color _defaultThumbnailColor;

        public void Init(Ability ability)
        {
            _ability = ability;
            nameLabel.text = ability.AbilityBaseData.UIName;
            _defaultThumbnailSprite = thumbnail.sprite; // technically this could be saved in some static way
            _defaultThumbnailColor = thumbnail.color;
            thumbnail.sprite = ability.AbilityBaseData.Thumbnail;
            thumbnail.color = Color.white;
            UpdateCooldownIndicator(ability.RemainingCooldown);
            ability.OnRemainingCooldownChanged += UpdateCooldownIndicator;
        }

        private void UpdateCooldownIndicator(float remainingCooldown)
        {
            cooldownIndicator.transform.localScale =
                    new Vector3(1, remainingCooldown / _ability.AbilityBaseData.MaxCooldown, 1);
            if (remainingCooldown < Mathf.Epsilon)
                cooldownText.text = "";
            else
            {
                if (remainingCooldown < 10)
                    cooldownText.text = $"{remainingCooldown:0.0}s";
                else if (remainingCooldown < 100)
                    cooldownText.text = $"{remainingCooldown}s";
                else
                {
                    int minutes = (int)(remainingCooldown / 60);
                    cooldownText.text = $"{minutes}m";
                }
            }
        }

        public void UnInit()
        {
            _ability.OnRemainingCooldownChanged -= UpdateCooldownIndicator;
            nameLabel.text = "";
            _ability = null;
            thumbnail.sprite = _defaultThumbnailSprite;
            thumbnail.color = _defaultThumbnailColor;
            cooldownText.text = "";
            cooldownIndicator.transform.localScale = Vector3.one;
        }
    }
}