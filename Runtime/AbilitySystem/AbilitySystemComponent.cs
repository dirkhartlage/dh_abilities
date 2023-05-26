using System;
using System.Collections.Generic;
using System.Data;
using DHAbilities.ResourceSystem;
using UnityEngine;

namespace DHAbilities.AbilitySystem
{
    public class AbilitySystemComponent : MonoBehaviour
    {
        public event Action OnUpdate;
        public event Action<Ability> OnAbilityAdded;
        public event Action<Ability> OnAbilityRemoved;

        public readonly Dictionary<int, Ability> AvailableAbilities = new Dictionary<int, Ability>();

        private readonly Dictionary<Ability, int>
                _availableAbilitiesToIndex = new Dictionary<Ability, int>(); // backwards mapping

        [Tooltip("specify all abilities that shall be available for this system by providing the base data object")]
        public AbilityBaseData[] availableAbilitiesBaseData;

        private ResourcePoolSystemComponent _resourcePoolSystemComponent;
        private bool _useResourceSystem = false;
        private int _availableAbilityIndexRunningCount = 0;

        /// <param name="abilityIndex">Identifies which of the available abilities is supposed to be activated</param>
        /// <param name="code">Code that is passed to the <see cref="DHAbilities.AbilitySystem.Ability"/></param>
        /// <returns>Weather activation was successful</returns>
        public bool TryUseAbility(int abilityIndex, int code)
        {
            if (!AvailableAbilities.ContainsKey(abilityIndex))
                return false;

            Ability selectedAbility = AvailableAbilities[abilityIndex];

            if (!selectedAbility.IsReady)
                return false;

            Dictionary<string, float> requiredResources = selectedAbility.RequiredResources;
            if (_useResourceSystem
                && !_resourcePoolSystemComponent.CheckResourcesAvailable(requiredResources)) // enough resources?
                return false;

            if (!selectedAbility.Actuate(code)) // actuate skill and check if succeeded
                return false;

            if (_useResourceSystem)
                _resourcePoolSystemComponent.DeductResources(requiredResources);
            selectedAbility.TriggerCooldown();
            return true;
        }

        // TODO: decide weather this or using the index is better
        public bool TryUseAbility(Ability ability, int code)
        {
            // TODO: I think this condition indicates programming errors and should be a conditionally compiled Assertion
            if (!_availableAbilitiesToIndex.ContainsKey(ability))
                return false;

            if (!ability.IsReady)
                return false;

            Dictionary<string, float> requiredResources = ability.RequiredResources;
            if (_useResourceSystem
                && !_resourcePoolSystemComponent.CheckResourcesAvailable(requiredResources)) // enough resources?
                return false;

            if (!ability.Actuate(code)) // actuate skill and check if succeeded
                return false;

            if (_useResourceSystem)
                _resourcePoolSystemComponent.DeductResources(requiredResources);
            ability.TriggerCooldown();
            return true;
        }

        public int AddAbility(AbilityBaseData abilityBaseData)
        {
            Ability ability = CreateAbilityFromBaseData(abilityBaseData);
            AvailableAbilities.Add(_availableAbilityIndexRunningCount, ability);
            _availableAbilitiesToIndex.Add(ability, _availableAbilityIndexRunningCount);
            OnAbilityAdded?.Invoke(ability);
            return _availableAbilityIndexRunningCount++;
        }

        public void RemoveAbility(Ability ability)
        {
            AvailableAbilities.Remove(_availableAbilitiesToIndex[ability]);
            _availableAbilitiesToIndex.Remove(ability);
            OnAbilityRemoved?.Invoke(ability);
        }

        public void RemoveAbility(int abilityIndex)
        {
            Ability ability = AvailableAbilities[abilityIndex];
            _availableAbilitiesToIndex.Remove(ability);
            AvailableAbilities.Remove(abilityIndex);
            OnAbilityRemoved?.Invoke(ability);
        }

        private void Awake()
        {
            _resourcePoolSystemComponent = GetComponent<ResourcePoolSystemComponent>();
            if (_resourcePoolSystemComponent != null)
                _useResourceSystem = true;

            foreach (AbilityBaseData abilityBaseData in availableAbilitiesBaseData)
                AddAbility(abilityBaseData);
            availableAbilitiesBaseData = null; // clear up memory. we don't need this anymore
        }

        private Ability CreateAbilityFromBaseData(AbilityBaseData abilityBaseData)
        {
            Ability ability;
            Type type;
            try
            {
                type = Type.GetType($"{abilityBaseData.Type}, {abilityBaseData.AssemblyName}");
            }
            catch (Exception e)
            {
                throw new DataException(
                        "Couldn't resolve the namespace.classname or assembly name specification for: " +
                        abilityBaseData.name, e);
            }

            if (type == null || !typeof(Ability).IsAssignableFrom(type))
                throw new ArgumentException("Invalid ability type: " + type);

            try
            {
                ability = Activator.CreateInstance(type, new object[] { this, abilityBaseData }) as Ability;
            }
            catch (Exception
                   e) // TODO: I think this may indicate only package internal problems, maybe make this conditionally compiled
            {
                throw new ConstraintException(
                        "Couldn't use the obligatory constructor with the signature described in the docs for one of your abilities.",
                        e);
            }

            return ability;
        }

        private void Update() => OnUpdate?.Invoke();

        private void OnDestroy()
        {
            foreach (var x in AvailableAbilities)
                x.Value.Dispose();
        }
    }
}