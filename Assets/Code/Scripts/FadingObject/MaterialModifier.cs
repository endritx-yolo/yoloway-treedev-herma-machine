using UnityEngine;

public class MaterialModifier : MonoBehaviour
{
    private PartItem _partItem;
    private MaterialModifierSO _highlightMaterialModifier;

    private Renderer   _renderer;
    private Material[] _materialArray;
    private Material[] _highlightedMaterialArray;
    private Material[] _transparentMaterialArray;
    private Material[] _selectionMaterialArray;


    private readonly string _highlightSOPath = "ScriptableObjects/Data/HighlightMaterialModifier";

    private bool _isTransparent;
    private bool _isSelected;

    private void Awake()
    {
        _highlightMaterialModifier = Resources.Load<MaterialModifierSO>(_highlightSOPath);

        _renderer = GetComponent<Renderer>();
        _partItem = GetComponent<PartItem>();
        
        _materialArray            = new Material[_renderer.materials.Length];
        _highlightedMaterialArray = new Material[_renderer.materials.Length];
        _transparentMaterialArray = new Material[_renderer.materials.Length];
        _selectionMaterialArray = new Material[_renderer.materials.Length];

        for (int i = 0; i < _highlightedMaterialArray.Length; i++)
        {
            _highlightedMaterialArray[i] = _highlightMaterialModifier.HighlightMaterial;
            _transparentMaterialArray[i] = _highlightMaterialModifier.TransparentMaterial;
            _selectionMaterialArray[i] = _highlightMaterialModifier.SelectionMaterial;
        }

        for (int i = 0; i < _renderer.materials.Length; i++)
            _materialArray[i] = _renderer.materials[i];
    }

    private void OnEnable() 
    {
        _partItem.OnSelect += MarkSelected;
        _partItem.OnDeSelect += UnMarkSelected;
    }

    private void OnDisable() 
    {
        _partItem.OnSelect -= MarkSelected;
        _partItem.OnDeSelect -= UnMarkSelected;
    } 

    public void Highlight() 
    {
        if (_isTransparent) return; 
        if (_isSelected) return; 
        _renderer.materials = _highlightedMaterialArray; 
    }

    public void Dehighlight() 
    {
        if (_isTransparent) return; 
        if (_isSelected) return; 
        _renderer.materials = _materialArray;
    }

    public void MakeTransparent() 
    { 
        if (_isTransparent) return; 
        if (_isSelected) return; 
        _isTransparent = true;
        _renderer.materials = _transparentMaterialArray; 
    }

    public void MakeOpaque() 
    {
        _isTransparent = false;
        _renderer.materials = _materialArray;
    }

    public void MarkSelected()
    {
        if (_isSelected) return;
        _isSelected = true;
        _renderer.materials = _selectionMaterialArray; 
    }

    public void UnMarkSelected()
    {
        _isSelected = false;
        _renderer.materials = _materialArray;
    }
}