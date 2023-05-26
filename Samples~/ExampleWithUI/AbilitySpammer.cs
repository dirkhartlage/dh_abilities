using System.Collections.Generic;
using DHAbilities.AbilitySystem;
using UnityEngine;

/// <summary>
/// Example activation for showcase. You can delete this script.
/// </summary>
[RequireComponent(typeof(AbilitySystemComponent))]
public sealed class AbilitySpammer : MonoBehaviour
{
    public List<int> abilitiesToSpam;

    private AbilitySystemComponent _abilitySystemComponent;

    private void Awake() => _abilitySystemComponent = GetComponent<AbilitySystemComponent>();

    // Update is called once per frame
    private void Update()
    {
        foreach (int i in abilitiesToSpam) 
            _abilitySystemComponent.TryUseAbility(i, 0);
    }
}
