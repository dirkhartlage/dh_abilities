using UnityEngine;

namespace DHAbilities.ResourceSystem
{
    [CreateAssetMenu(fileName = "ResourcePoolBaseData", menuName = "Abilities/ResourcePoolBaseData")]
    public class ResourcePoolBaseData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public float MaxResource { get; private set; }
        [field: SerializeField] public float StartingResource { get; private set; }
        [field: SerializeField] public float PassiveRegeneration { get; private set; }
        [field: Header("UI")]
        [field: SerializeField] public string UIName { get; private set; }
        [field: SerializeField] public Color UIBarColor { get; private set; }
    }
}