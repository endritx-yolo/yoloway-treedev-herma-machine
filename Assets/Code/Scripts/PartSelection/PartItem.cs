using UnityEngine;
using System;
using Lean.Common;
using Lean.Touch;
using Cinemachine;

public class PartItem : MonoBehaviour, ISelectableItem
{
    public static event Action<PartItem> OnAnySelected;
    public static event Action OnAnyDeSelected;
    public event Action OnSelect;
    public event Action OnDeSelect;
    public event Action OnHighlighted;
    public event Action OnUnHighlighted;

    private MaterialModifier _materialModifier;
    
    private Collider[] _colliderArray;

    private void Awake()
    {
        _colliderArray    = GetComponents<Collider>();
        _materialModifier = GetComponent<MaterialModifier>();
    }

    public void Select()
    {
        OnAnySelected?.Invoke(this);
        OnSelect?.Invoke();
    }
    
    public void Deselect()
    {
        OnAnyDeSelected?.Invoke();
        OnDeSelect?.Invoke();
    }

    public void Highlight()
    {
        OnHighlighted?.Invoke();
        _materialModifier.Highlight();
    }
    
    public void DeHighlight()
    {
        OnUnHighlighted?.Invoke();
        _materialModifier.Dehighlight();
    }

    public void Hide()
    {
        _materialModifier.MakeTransparent();
    }

    public void Show()
    {
        _materialModifier.MakeOpaque();
    }
    
    public void EnableColliders()
    {
        for (int i = 0; i < _colliderArray.Length; i++)
            _colliderArray[i].enabled = true;
    }

    public void DisableColliders()
    {
        for (int i = 0; i < _colliderArray.Length; i++)
            _colliderArray[i].enabled = false;
    }

    private void OnMouseOver()
    {
        Highlight();
    }

    private void OnMouseExit()
    {
        DeHighlight();
    }
}