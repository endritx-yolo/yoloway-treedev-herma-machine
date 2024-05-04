using UnityEngine;
using System;

public class ParentGroup : MonoBehaviour, ISelectableItem
{
    public static event Action<ParentGroup> OnAnySelected;
    public static event Action              OnAnyDeSelected;

    private Collider[]   _colliderArray;
    private PartsGroup[] _partsGroupArray;

    private void Awake()
    {
        _colliderArray   = GetComponents<Collider>();
        _partsGroupArray = GetComponentsInChildren<PartsGroup>();
    }

    private void Start() => DisableChildrenColliders();

    public void Select()
    {
        OnAnySelected?.Invoke(this);
        DisableColliders();
        EnableChildrenColliders();
    }

    public void Deselect()
    {
        OnAnyDeSelected?.Invoke();
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
}