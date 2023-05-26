using System;
using System.Collections.Generic;
using UnityEngine;

namespace DHAbilities.AbilitySystem
{
    public abstract class Ability : IDisposable
    {
        public event Action<float> OnRemainingCooldownChanged;

        public float RemainingCooldown
        {
            get => _remainingCooldown;
            set
            {
                _remainingCooldown = value;
                OnRemainingCooldownChanged?.Invoke(value);
            }
        }

        public bool IsReady => RemainingCooldown < Mathf.Epsilon;

        public void TriggerCooldown()
        {
            if (_maxCooldown < Mathf.Epsilon)
                return;

            RemainingCooldown = _maxCooldown;
            OwningSystem.OnUpdate += UpdateCooldown;
        }

        public Dictionary<string, float> RequiredResources;

        protected readonly AbilitySystemComponent OwningSystem;
        public readonly AbilityBaseData AbilityBaseData;

        private float _remainingCooldown;
        private readonly float _maxCooldown;

        protected Ability(in AbilitySystemComponent owningSystem, in AbilityBaseData abilityBaseData)
        {
            OwningSystem = owningSystem;
            AbilityBaseData = abilityBaseData;

            // init from base data
            _maxCooldown = abilityBaseData.MaxCooldown;
            RemainingCooldown = abilityBaseData.InitialCooldown;
            if (RemainingCooldown > Mathf.Epsilon)
                OwningSystem.OnUpdate += UpdateCooldown;
            RequiredResources = abilityBaseData.RequiredResources;
        }

        /// <returns>Weather the skill was actually successfully activated</returns>
        /// <param name="code">Can be used to pass additional information.
        /// E.g: if you can left click to confirm and right click to cancel, you could assign a code for each</param>
        // Note: this is where more generalized behaviour could go in the future
        public bool Actuate(int code) => Behaviour(code);

        /// <returns>Weather the skill was actually successfully activated</returns>
        /// <param name="code">Can be used to pass additional information.
        /// E.g: if you can left click to confirm and right click to cancel, you could assign a code for each</param>
        // Note: this is where more generalized behaviour could go in the future
        protected abstract bool Behaviour(int code);

        // TODO: I think this would indicate a package internal problem, so I think this can be safely deleted
        ~Ability()
        {
            throw new IncorrectUsageException("Dispose was not called");
        }

        public void Dispose()
        {
            OwningSystem.OnUpdate -= UpdateCooldown;
            GC.SuppressFinalize(this);
        }


        private void UpdateCooldown()
        {
            if (RemainingCooldown - Time.deltaTime < Mathf.Epsilon)
            {
                RemainingCooldown = 0f;
                OwningSystem.OnUpdate -= UpdateCooldown;
                return;
            }

            RemainingCooldown -= Time.deltaTime;
        }
    }
}