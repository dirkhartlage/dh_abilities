using System.Collections.Generic;
using DHAbilities.AbilitySystem;
using UnityEngine;

namespace DHAbilities.Samples.UI.Abilities
{
    public sealed class Ability2 : Ability
    {
        public Ability2(in AbilitySystemComponent owningSystem, in AbilityBaseData abilityBaseData) : base(in owningSystem, in abilityBaseData)
        {
            // set requiredResources here if you don't have OdinInspector and are therefore unable to set it in the ScriptableObject
            RequiredResources = new Dictionary<string, float>
            {
                    { "rage", 26 },
                    { "stamina", 33 }
            };
        }

        protected override bool Behaviour(int code)
        {
            Debug.Log("used: " + nameof(Ability2));
            return true;
        }
    }
}
