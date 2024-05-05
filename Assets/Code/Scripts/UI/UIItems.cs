using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIItems : MonoBehaviour
{
    public static event Action OnAnyDeselectItem;

    [SerializeField] private GameObject _groupPanel;
    [SerializeField] private Button _backButton;

    private UIMachineGroup[] _uiGroupArray;
    private UIMachineSubGroup[] _subGroupArray;
    private UIMachineItem[] _uiItemArray;

    private UIMachineItem _highlightedUIItem;

    [SerializeField]private GameObject _subGroup;
    [SerializeField]private GameObject _item;

    private void Awake()
    {
        _backButton.onClick.AddListener(DeselectItem);
        _uiGroupArray = GetComponentsInChildren<UIMachineGroup>(true);
        _subGroupArray = GetComponentsInChildren<UIMachineSubGroup>(true);
        _uiItemArray = GetComponentsInChildren<UIMachineItem>(true);
    }

    private void OnEnable()
    {
        UIMachineGroup.OnAnySelectUIGroup += AssignSubGroup;
        UIMachineSubGroup.OnAnySelectUISubGroup += AssignItem;
        UIMachineItem.OnUIItemHighlighted += AssignHighlightedItem;

        PartItem.OnAnySelected    += HighlightItemButton;
        ParentGroup.OnAnySelected += OnGroupSelected;
        PartsGroup.OnAnySelected += OnSubGroupSelected;

        //PartSelectionController.OnAnyDeselectItem += UpdateUIStep;
    }

    private void OnDisable()
    {
        UIMachineGroup.OnAnySelectUIGroup -= AssignSubGroup;
        UIMachineSubGroup.OnAnySelectUISubGroup -= AssignItem;
        UIMachineItem.OnUIItemHighlighted -= AssignHighlightedItem;

        PartItem.OnAnySelected    -= HighlightItemButton;
        ParentGroup.OnAnySelected -= OnGroupSelected;
        PartsGroup.OnAnySelected -= OnSubGroupSelected;

        //PartSelectionController.OnAnyDeselectItem -= UpdateUIStep;
    }

    private void DeselectItem()
    {
        if (_item != null)
        {
            _item.SetActive(false);
            _subGroup.SetActive(true);
            _item = null;
            //OnAnyDeselectItem?.Invoke();
            OnAnyDeselectItem?.Invoke();
            if (_highlightedUIItem == null) return;
            _highlightedUIItem.Deselect();
            _highlightedUIItem = null;
            OnAnyDeselectItem?.Invoke();
            return;
        }

        if (_subGroup != null)
        {
            _subGroup.SetActive(false);
            _groupPanel.SetActive(true);
            _backButton.gameObject.SetActive(false);
            _subGroup = null;
            OnAnyDeselectItem?.Invoke();
            return;
        }
    }

    private void AssignSubGroup(GameObject subgroup)
    {
        _backButton.gameObject.SetActive(true);
        _groupPanel.SetActive(false);
        _subGroup = subgroup;
    }

    private void AssignItem(GameObject item)
    {
        _subGroup.SetActive(false);
        _item = item;
    }

    private void HighlightItemButton(PartItem partItem)
    {
        for(int i = 0; i < _uiItemArray.Length; i++)
        {
            if (_uiItemArray[i].PartItem.Equals(partItem))
            {
                _uiItemArray[i].Select();
            }
            else
                _uiItemArray[i].Deselect();
        }
    }

    private void AssignHighlightedItem(UIMachineItem item)
    {
        _highlightedUIItem = item;
    }

    private void OnGroupSelected(ParentGroup parentGroup)
    {
        var group = _uiGroupArray.Where(x => x != null && x.ParentGroup != null && x.ParentGroup.Equals(parentGroup)).FirstOrDefault();
        group.EnableGroupFrom3DInput();
    }

    private void OnSubGroupSelected(PartsGroup partGroup)
    {
        var subGroup = _subGroupArray.Where(x => x != null && x.PartsGroup != null && x.PartsGroup.Equals(partGroup)).FirstOrDefault();
        subGroup.EnableSubGroupFrom3DInput();
    }
}
