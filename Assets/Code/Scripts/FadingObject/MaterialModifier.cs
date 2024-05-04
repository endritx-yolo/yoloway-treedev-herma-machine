using UnityEngine;

public class MaterialModifier : MonoBehaviour
{
    private MaterialModifierSO _highlightMaterialModifier;

    private Renderer   _renderer;
    private Material[] _materialArray;
    private Material[] _highlightedMaterilArray;

    private readonly string _highlightSOPath = "ScriptableObjects/Data/HighlightMaterialModifier";

    private void Awake()
    {
        _highlightMaterialModifier = Resources.Load<MaterialModifierSO>(_highlightSOPath);

        _renderer                = GetComponent<Renderer>();
        _materialArray           = new Material[_renderer.materials.Length];
        _highlightedMaterilArray = new Material[_renderer.materials.Length];

        for (int i = 0; i < _highlightedMaterilArray.Length; i++)
            _highlightedMaterilArray[i] = _highlightMaterialModifier.HighlightMaterial;

        for (int i = 0; i < _renderer.materials.Length; i++)
            _materialArray[i] = _renderer.materials[i];
    }

    public void Highlight() { _renderer.materials = _highlightedMaterilArray; }

    public void Dehighlight() { _renderer.materials = _materialArray; }
}