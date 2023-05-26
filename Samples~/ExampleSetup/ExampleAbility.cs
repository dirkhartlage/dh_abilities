using System.Collections.Generic;
using DHAbilities.AbilitySystem;
using UnityEngine;

namespace DHAbilities.Samples
{
    public sealed class ExampleAbility1 : Ability
    {
        public ExampleAbility1(in AbilitySystemComponent owner, in AbilityBaseData abilityBaseData) : base(in owner,
                in abilityBaseData)
        {
            // set requiredResources here if you don't have OdinInspector and are therfore unable to set it in the ScriptableObject
            RequiredResources = new Dictionary<string, float>
            {
                    { "example", 42 }
            };
        }

        /// <inheritdoc/>
        protected override bool Behaviour(int code)
        {
            // implement custom behaviour here
            // tip: you can use owner.Instantiate to spawn a Prefab from here if necessary
            Debug.Log("Kaboom");
            return true;
        }
    }
}