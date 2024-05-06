using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class SelectedItemsTracker : MonoBehaviour
{
    public static event Action<ParentGroup> OnAnyGroupAssigned;
    public static event Action<PartsGroup> OnAnySubGroupAssigned;
    public static event Action<PartItem> OnAnyItemAssigned;

    private List<ParentGroup> _parentGroupList = new List<ParentGroup>();
    private List<PartsGroup>  _partsGroupList  = new List<PartsGroup>();
    private List<PartItem>    _itemList        = new List<PartItem>();

    [SerializeField] private ParentGroup _selectedParentGroup;
    [SerializeField] private PartsGroup  _selectedPartsGroup;
    [SerializeField] private PartItem    _selectedPartItem;

    #region Properties

    public ParentGroup SelectedParentGroup 
    {
        get =>  _selectedParentGroup;
        set
        {
            _selectedParentGroup = value;
            OnAnyGroupAssigned?.Invoke(_selectedParentGroup);
        }
    }

    public PartsGroup SelectedPartsGroup 
    {
        get =>  _selectedPartsGroup;
        set
        {
            _selectedPartsGroup = value;
            OnAnySubGroupAssigned?.Invoke(_selectedPartsGroup);
        }
    }

    public PartItem SelectedPartItem 
    {
        get =>  _selectedPartItem;
        set
        {
            _selectedPartItem = value;
            OnAnyItemAssigned?.Invoke(_selectedPartItem);
        }
    }

    #endregion

    private void Awake()
    {
        _parentGroupList = FindObjectsOfType<ParentGroup>().ToList();
        _partsGroupList  = FindObjectsOfType<PartsGroup>().ToList();
        _itemList        = FindObjectsOfType<PartItem>().ToList();
    }

    public void OnEnable()
    {
        ParentGroup.OnAnySelected += SelectParentGroup;
        PartsGroup.OnAnySelected  += SelectPartsGroup;
        PartItem.OnAnySelected    += SelectItem;

        ParentGroup.OnAnyDeSelected += DeselectParentGroup;
        PartsGroup.OnAnyDeSelected  += DeselectPartsGroup;
        PartItem.OnAnyDeSelected    += DeselectItem;

        PartSelectionController.OnAnyDeselectItem += DeselectAnyItem;

        UIItems.OnAnyDeselectItem += DeselectAnyItem;
    }

    public void OnDisable()
    {
        ParentGroup.OnAnySelected -= SelectParentGroup;
        PartsGroup.OnAnySelected  -= SelectPartsGroup;
        PartItem.OnAnySelected    -= SelectItem;

        ParentGroup.OnAnyDeSelected -= DeselectParentGroup;
        PartsGroup.OnAnyDeSelected  -= DeselectPartsGroup;
        PartItem.OnAnyDeSelected    -= DeselectItem;

        PartSelectionController.OnAnyDeselectItem -= DeselectAnyItem;

        UIItems.OnAnyDeselectItem -= DeselectAnyItem;
    }

    private void SelectParentGroup(ParentGroup parentGroup)
    {
        SelectedParentGroup = parentGroup;
        for (int i = 0; i < _parentGroupList.Count; i++)
        {
            if (_parentGroupList[i].Equals(SelectedParentGroup)) continue;
            _parentGroupList[i].Hide();
        }
    }

    private void SelectPartsGroup(PartsGroup partsGroup)
    {
        SelectedPartsGroup = partsGroup;
        for (int i = 0; i < _partsGroupList.Count; i++)
        {
            if (_partsGroupList[i].Equals(SelectedPartsGroup)) continue;
            _partsGroupList[i].Hide();
        }
    }

    private void SelectItem(PartItem partItem)
    {
        if (SelectedPartItem != null)
            SelectedPartItem.Deselect();
        SelectedPartItem = partItem;
    }

    private void DeselectParentGroup()
    {
        SelectedParentGroup.DisableChildrenColliders();
        SelectedParentGroup.EnableColliders();
        SelectedParentGroup = null;
    }

    private void DeselectPartsGroup()
    {
        SelectedPartsGroup.DisableChildrenColliders();
        SelectedParentGroup.EnableChildrenColliders();
        SelectedPartsGroup = null;
    }

    private void DeselectItem() 
    {
        SelectedPartItem = null; 
    }

    private void DeselectAnyItem()
    {
        if (SelectedPartItem != null)
        {
            SelectedPartItem.Deselect();
            SelectedPartItem = null;
            SelectedPartsGroup.Show();
            return;
        }

        if (SelectedPartsGroup != null)
        {
            SelectedPartsGroup.Deselect();
            SelectedPartsGroup = null;
            SelectedParentGroup.Show();
            return;
        }

        if (SelectedParentGroup != null)
        {
            SelectedParentGroup.Deselect();
            SelectedParentGroup = null;

            for (int i = 0; i < _parentGroupList.Count; i++)
                _parentGroupList[i].Show();

            return;
        }
    }
}