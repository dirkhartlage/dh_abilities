using System.Collections.Generic;
using System.Data;
using DHAbilities.AbilitySystem;
using UnityEngine;

namespace DHAbilities.Samples.UI.QuickslotPanel
{
    [DefaultExecutionOrder(1000)]
    public sealed class UIQuickslotManager : MonoBehaviour
    {
        [SerializeField] private AbilitySystemComponent abilitySystemComponent;
        [SerializeField] private UIQuickslot[] availableQuickslots;

        private void OnEnable()
        {
            int i = 0;
            if (availableQuickslots.Length < abilitySystemComponent.AvailableAbilities.Count)
                throw new ConstraintException("too many abilities");
            foreach (KeyValuePair<int, Ability> availableAbility in abilitySystemComponent.AvailableAbilities)
                availableQuickslots[i++].Init(availableAbility.Value);
        }
    }
}