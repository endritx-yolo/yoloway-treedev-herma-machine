using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SelectedItemsTracker : MonoBehaviour
{
    private List<ParentGroup> _parentGroupList = new List<ParentGroup>();
    private List<PartsGroup>  _partsGroupList  = new List<PartsGroup>();
    private List<PartItem>    _itemList        = new List<PartItem>();

    [SerializeField] private ParentGroup _selectedParentGroup;
    [SerializeField] private PartsGroup  _selectedPartsGroup;
    [SerializeField] private PartItem    _selectedPartItem;

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
    }

    private void SelectParentGroup(ParentGroup parentGroup)
    {
        _selectedParentGroup = parentGroup;
        for (int i = 0; i < _parentGroupList.Count; i++)
        {
            if (_parentGroupList[i].Equals(_selectedParentGroup)) continue;
            _parentGroupList[i].Hide();
        }
    }

    private void SelectPartsGroup(PartsGroup partsGroup)
    {
        _selectedPartsGroup = partsGroup;
        for (int i = 0; i < _partsGroupList.Count; i++)
        {
            if (_partsGroupList[i].Equals(_selectedPartsGroup)) continue;
            _partsGroupList[i].Hide();
        }
    }

    private void SelectItem(PartItem partItem)
    {
        _selectedPartItem = partItem;
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].Equals(_selectedPartItem)) continue;
            _itemList[i].Hide();
        }
    }

    private void DeselectParentGroup()
    {
        _selectedParentGroup.DisableChildrenColliders();
        _selectedParentGroup.EnableColliders();
        _selectedParentGroup = null;
    }

    private void DeselectPartsGroup()
    {
        _selectedPartsGroup.DisableChildrenColliders();
        _selectedParentGroup.EnableChildrenColliders();
        _selectedPartsGroup = null;
    }

    private void DeselectItem() { _selectedPartItem = null; }

    private void DeselectAnyItem()
    {
        if (_selectedPartItem != null)
        {
            _selectedPartItem.Deselect();
            _selectedPartItem = null;
            return;
        }

        if (_selectedPartsGroup != null)
        {
            _selectedPartsGroup.Deselect();
            _selectedPartsGroup = null;
            return;
        }

        if (_selectedParentGroup != null)
        {
            _selectedParentGroup.Deselect();
            _selectedParentGroup = null;
            return;
        }
    }
}