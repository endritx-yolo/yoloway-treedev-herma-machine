using UnityEngine;

[CreateAssetMenu(fileName = "MaterialModifierSO", menuName = "SO/MaterialModifier", order = 0)]
public class MaterialModifierSO : ScriptableObject
{
    [SerializeField] private Material _highlightMaterial;
    
    public Material HighlightMaterial => _highlightMaterial;
}
