using UnityEngine;
using System;

public class PartItem : MonoBehaviour, ISelectableItem
{
    public static event Action<PartItem> OnAnySelected;
    public static event Action OnAnyDeSelected;
    
    private Collider[] _colliderArray;

    private void Awake() { _colliderArray = GetComponents<Collider>(); }

    public void Select()
    {
        OnAnySelected?.Invoke(this);
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
}