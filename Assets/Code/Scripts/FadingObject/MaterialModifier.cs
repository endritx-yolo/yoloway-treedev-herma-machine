using UnityEngine;

public class MaterialModifier : MonoBehaviour
{
    private MaterialModifierSO _highlightMaterialModifier;

    private Renderer   _renderer;
    private Material[] _materialArray;
    private Material[] _highlightedMaterialArray;
    private Material[] _transparentMaterialArray;

    private readonly string _highlightSOPath = "ScriptableObjects/Data/HighlightMaterialModifier";

    private void Awake()
    {
        _highlightMaterialModifier = Resources.Load<MaterialModifierSO>(_highlightSOPath);

        _renderer                 = GetComponent<Renderer>();
        _materialArray            = new Material[_renderer.materials.Length];
        _highlightedMaterialArray = new Material[_renderer.materials.Length];
        _transparentMaterialArray = new Material[_renderer.materials.Length];

        for (int i = 0; i < _highlightedMaterialArray.Length; i++)
            _highlightedMaterialArray[i] = _highlightMaterialModifier.HighlightMaterial;

        for (int i = 0; i < _transparentMaterialArray.Length; i++)
            _transparentMaterialArray[i] = _highlightMaterialModifier.TransparentMaterial;

        for (int i = 0; i < _renderer.materials.Length; i++)
            _materialArray[i] = _renderer.materials[i];
    }

    public void Highlight() { _renderer.materials = _highlightedMaterialArray; }

    public void Dehighlight() { _renderer.materials = _materialArray; }

    public void MakeTransparent() { _renderer.materials = _transparentMaterialArray; }
}