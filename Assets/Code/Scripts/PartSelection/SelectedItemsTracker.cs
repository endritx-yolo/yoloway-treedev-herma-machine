using UnityEngine;

public class SelectedItemsTracker : MonoBehaviour
{
    [SerializeField] private ParentGroup _selectedParentGroup;
    [SerializeField] private PartsGroup  _selectedPartsGroup;
    [SerializeField] private PartItem    _selectedPartItem;

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

    private void SelectParentGroup(ParentGroup parentGroup) => _selectedParentGroup = parentGroup;
    private void SelectPartsGroup(PartsGroup   partsGroup)  => _selectedPartsGroup = partsGroup;
    private void SelectItem(PartItem           partItem)    => _selectedPartItem = partItem;

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

    private void DeselectItem()
    {
        _selectedPartItem = null;
    }

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