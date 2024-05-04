using UnityEngine;

[CreateAssetMenu(fileName = "MaterialModifierSO", menuName = "SO/MaterialModifier", order = 0)]
public class MaterialModifierSO : ScriptableObject
{
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private Material _transparentMaterial;
    
    public Material HighlightMaterial    => _highlightMaterial;
    public Material TransparentMaterial => _transparentMaterial;
}
