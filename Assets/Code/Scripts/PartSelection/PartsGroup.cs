using UnityEngine;
using System;

public class PartsGroup : MonoBehaviour, ISelectableItem
{
    public static event Action<PartsGroup> OnAnySelected;
    public static event Action             OnAnyDeSelected;
    public event Action OnHighlighted;
    public event Action OnUnHighlighted;

    private ParentGroup _parentGroup;

    private Collider[] _colliderArray;
    private PartItem[] _partItemArray;
    private Renderer[] _renderers;

    #region Properties

    public ParentGroup ParentGroup { get => _parentGroup; set => _parentGroup = value; }

    #endregion

    private void Awake()
    {
        _colliderArray = GetComponents<Collider>();
        _partItemArray = GetComponentsInChildren<PartItem>();
        _renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start() => DisableChildrenColliders();

    public void Select()
    {
        OnAnySelected?.Invoke(this);
        ParentGroup.DisableChildrenCollidersExceptTheSelectedOne(this);
        DisableColliders();
        EnableChildrenColliders();
    }

    public void Deselect() { OnAnyDeSelected?.Invoke(); }

    public void Highlight()
    {
        OnHighlighted?.Invoke();
        for (int i = 0; i < _partItemArray.Length; i++)
            _partItemArray[i].Highlight();
    }

    public void DeHighlight()
    {
        OnUnHighlighted?.Invoke();
        for (int i = 0; i < _partItemArray.Length; i++)
            _partItemArray[i].DeHighlight();
    }

    public void Hide()
    {
        for (int i = 0; i < _partItemArray.Length; i++)
            _partItemArray[i].Hide();
    }

    public void Show()
    {
         for (int i = 0; i < _partItemArray.Length; i++)
         _partItemArray[i].Show();
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
        for (int i = 0; i < _partItemArray.Length; i++)
            _partItemArray[i].EnableColliders();
    }

    public void DisableChildrenColliders()
    {
        for (int i = 0; i < _partItemArray.Length; i++)
            _partItemArray[i].DisableColliders();
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
}