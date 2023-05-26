using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace DHAbilities.AbilitySystem
{
    [CreateAssetMenu(fileName = "AbilityBaseData", menuName = "Abilities/AbilityBaseData")]
    public class AbilityBaseData : ScriptableObject
    {
        [SerializeField] private string identifier;
        public string Identifier => identifier;

        [Tooltip("Full type qualifier of Ability class, i.e.: <full namespace>.<classname>")]
        [SerializeField] private string classType = "<namespace>.<classname>";
        public string Type { get => classType; private set => classType = value; }

        [SerializeField]
        private string description;
        public string Description => description;

        [SerializeField]
        private Sprite thumbnail;
        public Sprite Thumbnail => thumbnail;

        [SerializeField]
        private float maxCooldown;
        public float MaxCooldown => maxCooldown;

        [SerializeField]
        private float initialCooldown;
        public float InitialCooldown => initialCooldown;

        [SerializeField]
        private string uiName;
        public string UIName => uiName;
        
        // only available with OdinInspector:
        [field: SerializeField] public Dictionary<string, float> RequiredResources { get; private set; }
        
        [Header("Advanced")]
        [Tooltip("Do not change, unless you know what you're doing")]
        [SerializeField] private string assemblyName = "Assembly-CSharp";
        public string AssemblyName => assemblyName;
    }
}