using UnityEngine;
using System;

public class PartsGroup : MonoBehaviour, ISelectableItem
{
    public static event Action<PartsGroup> OnAnySelected;
    public static event Action             OnAnyDeSelected;
    
    private Collider[] _colliderArray;
    private PartItem[] _partItems;

    private void Awake()
    {
        _colliderArray = GetComponents<Collider>();
        _partItems     = GetComponentsInChildren<PartItem>();
    }

    private void Start() => DisableChildrenColliders();
    
    public void Select()
    {
        OnAnySelected?.Invoke(this);
        DisableColliders();
        EnableChildrenColliders();
        Debug.Log($"Selected Group {gameObject.name}");
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
        for (int i = 0; i < _partItems.Length; i++)
            _partItems[i].EnableColliders();
    }

    public void DisableChildrenColliders()
    {
        for (int i = 0; i < _partItems.Length; i++)
            _partItems[i].DisableColliders();
    }
}