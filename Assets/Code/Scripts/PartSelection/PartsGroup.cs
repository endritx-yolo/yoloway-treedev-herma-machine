using UnityEngine;
using System;

public class PartsGroup : MonoBehaviour, ISelectableItem
{
    public static event Action<PartsGroup> OnAnySelected;
    public static event Action             OnAnyDeSelected;

    private ParentGroup _parentGroup;

    private Collider[] _colliderArray;
    private PartItem[] _partItemArray;

    #region Properties

    public ParentGroup ParentGroup { get => _parentGroup; set => _parentGroup = value; }

    #endregion

    private void Awake()
    {
        _colliderArray = GetComponents<Collider>();
        _partItemArray = GetComponentsInChildren<PartItem>();
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
        for (int i = 0; i < _partItemArray.Length; i++)
            _partItemArray[i].Highlight();
    }

    public void DeHighlight()
    {
        for (int i = 0; i < _partItemArray.Length; i++)
            _partItemArray[i].DeHighlight();
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

    private void OnMouseOver() { Highlight(); }

    private void OnMouseExit() { DeHighlight(); }
}