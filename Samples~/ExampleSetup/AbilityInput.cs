using DHAbilities.AbilitySystem;
using UnityEngine;

[RequireComponent(typeof(AbilitySystemComponent))]
public sealed class AbilityInput : MonoBehaviour
{
    private AbilitySystemComponent _abilitySystemComponent;

    private void Awake()
    {
        _abilitySystemComponent = GetComponent<AbilitySystemComponent>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            _abilitySystemComponent.TryUseAbility(0,0);
    }
}
