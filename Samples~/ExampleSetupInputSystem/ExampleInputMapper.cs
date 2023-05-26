using DHAbilities.AbilitySystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DHAbilities.Samples
{
    public class ExampleInputMapper : MonoBehaviour
    {
        private AbilitySystemComponent _abilitySystemComponent;
        private void Awake() => _abilitySystemComponent = GetComponent<AbilitySystemComponent>();

        public void TryUseAbility(InputAction.CallbackContext cb)
        {
            if (cb.started)
                _abilitySystemComponent.TryUseAbility(0, 0);
        }
    }
}