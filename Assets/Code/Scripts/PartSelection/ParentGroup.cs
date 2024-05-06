using UnityEngine;
using System;

public class ParentGroup : MonoBehaviour, ISelectableItem
{
    public static event Action<ParentGroup> OnAnySelected;
    public static event Action              OnAnyDeSelected;
    public event Action OnHighlighted;
    public event Action OnUnHighlighted;

    private Collider[]   _colliderArray;
    private PartsGroup[] _partsGroupArray;
    private Renderer[] _renderers;

    private void Awake()
    {
        _colliderArray   = GetComponents<Collider>();
        _partsGroupArray = GetComponentsInChildren<PartsGroup>();
        _renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < _partsGroupArray.Length; i++)
            _partsGroupArray[i].ParentGroup = this;
    }

    private void Start() => DisableChildrenColliders();

    public void Select()
    {
        OnAnySelected?.Invoke(this);
        DisableColliders();
        EnableChildrenColliders();
    }

    public void Deselect() { OnAnyDeSelected?.Invoke(); }

    public void Highlight()
    {
        OnHighlighted?.Invoke();
        for (int i = 0; i < _partsGroupArray.Length; i++)
            _partsGroupArray[i].Highlight();
    }

    public void DeHighlight()
    {
        OnUnHighlighted?.Invoke();
        for (int i = 0; i < _partsGroupArray.Length; i++)
            _partsGroupArray[i].DeHighlight();
    }

    public void Hide()
    {
        for (int i = 0; i < _partsGroupArray.Length; i++)
            _partsGroupArray[i].Hide();
    }

    public void Show()
    {
        for (int i = 0; i < _partsGroupArray.Length; i++)
            _partsGroupArray[i].Show();
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

    public void EnableChildrenColliders()
    {
        for (int i = 0; i < _partsGroupArray.Length; i++)
            _partsGroupArray[i].EnableColliders();
    }

    public void DisableChildrenColliders()
    {
        for (int i = 0; i < _partsGroupArray.Length; i++)
            _partsGroupArray[i].DisableColliders();
    }

    public void DisableChildrenCollidersExceptTheSelectedOne(PartsGroup partsGroup)
    {
        for (int i = 0; i < _partsGroupArray.Length; i++)
        {
            if (_partsGroupArray[i].Equals(partsGroup)) continue;
            _partsGroupArray[i].DisableColliders();
        }
    }

    public Vector3 GetBoundsCenterPosition()
    {
       Bounds bounds = _renderers[0].bounds;
       for(int i = 0; i < _renderers.Length; i++)
       {
            bounds = bounds.GrowBounds(_renderers[i].bounds);
       }
       Vector3 center = bounds.center;
       return center;
    }

    private void OnMouseOver() { Highlight(); }

    private void OnMouseExit() { DeHighlight(); }
    
    private void OnDrawGizmosSelected() 
    {
        if (_renderers.Length == 0) _renderers = GetComponentsInChildren<Renderer>();
       Gizmos.DrawWireSphere(GetBoundsCenterPosition(), .5f);
    }
}