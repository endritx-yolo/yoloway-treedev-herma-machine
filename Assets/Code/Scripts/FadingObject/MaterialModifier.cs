using UnityEngine;
using UnityEngine.Rendering;

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

    #region ShaderPropertyNames

    private readonly string _alphaPremultiplyOn = "_ALPHAPREMULTIPLY_ON";
    private readonly string _zWrite = "_ZWrite";
    private readonly string _renderType = "RenderType";
    private readonly string _transparent = "Transparent";
    private readonly string _opaque = "Opaque";
    private readonly string _color = "_Color";
    private readonly string _dstBlend = "_DstBlend";
    private readonly string _glossiness = "_Glossiness";
    private readonly string _metallic = "_Metallic";
    private readonly string _mode = "_Mode";

    #endregion

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
        //_renderer.materials = _transparentMaterialArray; 

        for (int i = 0; i < _renderer.materials.Length; i++)
        {
            Material renderMat = _renderer.materials[i];
            renderMat.SetInt(_zWrite, 0);
            renderMat.renderQueue = (int)RenderQueue.Transparent;
            renderMat.renderQueue = 3000;
            renderMat.SetOverrideTag(_renderType, _transparent);
            Color fadedColor = renderMat.color;
            fadedColor.a = 0f;
            renderMat.color = fadedColor;
            renderMat.EnableKeyword(_alphaPremultiplyOn);
            renderMat.SetFloat(_dstBlend, 10);
            renderMat.SetFloat(_metallic, 0.1f);
            renderMat.SetFloat(_glossiness, .5f);
            renderMat.SetInt(_mode, 3);
        }
    }

    public void MakeOpaque() 
    {
        _isTransparent = false;
        //_renderer.materials = _materialArray;

        for (int i = 0; i < _renderer.materials.Length; i++)
        {
            Material renderMat = _renderer.materials[i];
            renderMat.SetInt(_zWrite, 1);
            renderMat.renderQueue = (int)RenderQueue.Geometry;
            renderMat.renderQueue = -1;
            renderMat.SetOverrideTag(_renderType, _opaque);
            Color opaqueColor = renderMat.color;
            opaqueColor.a = 1f;
            renderMat.color = opaqueColor;
            renderMat.DisableKeyword(_alphaPremultiplyOn);
            renderMat.SetFloat(_dstBlend, 0);
            renderMat.SetFloat(_metallic, .9f);
            renderMat.SetFloat(_glossiness, .6f);
            renderMat.SetInt(_mode, 0);
        }
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