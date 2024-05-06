using System;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMachinePresenter : MonoBehaviour
{
    public static event Action<ParentGroup> OnAnyAddGroupToCart;
    public static event Action<PartsGroup> OnAnyAddSubGroupToCart;
    public static event Action<PartItem> OnAnyAddItemToCart;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _itemCountText;

    [BoxGroup("Buttons")] [SerializeField] private Button _addToCartButton;

    private readonly string _pricePrefix = "Price: ";
    private readonly string _pricePostFix = " Euro";

    [SerializeField] private ParentGroup _parentGroup;
    [SerializeField] private PartsGroup _partsGroup;
    [SerializeField] private PartItem _partItem;

    private void Awake() 
    {
        _itemCountText.text = "0";
        _panel.SetActive(false);

        _addToCartButton.onClick.AddListener(AddItemToCart);
    } 

    private void OnEnable()
    {
        SelectedItemsTracker.OnAnyGroupAssigned += OnGroupSelected;
        SelectedItemsTracker.OnAnySubGroupAssigned += OnSubGroupSelected;
        SelectedItemsTracker.OnAnyItemAssigned += OnItemSelected;
    }

    private void OnDisable()
    {
        SelectedItemsTracker.OnAnyGroupAssigned -= OnGroupSelected;
        SelectedItemsTracker.OnAnySubGroupAssigned -= OnSubGroupSelected;
        SelectedItemsTracker.OnAnyItemAssigned -= OnItemSelected;
    }

    private void OnGroupSelected(ParentGroup parentGroup)
    {
        _parentGroup = parentGroup;
        if (_parentGroup == null) 
        {
            _panel.SetActive(false);
            return;
        }
        _panel.SetActive(true);
        _itemNameText.text = parentGroup.gameObject.name;
        _priceText.text = $"{_pricePrefix}{parentGroup.Price}{_pricePostFix}";
    
        UpdateItemCountText();
    }

    private void OnSubGroupSelected(PartsGroup partsGroup)
    {
        _partsGroup = partsGroup;
        if (partsGroup == null)
        {
            OnGroupSelected(_parentGroup);
            return;
        }
        _panel.SetActive(true);
        _itemNameText.text = partsGroup.gameObject.name;
        _priceText.text = $"{_pricePrefix}{partsGroup.Price}{_pricePostFix}";
        UpdateItemCountText();
    }

    private void OnItemSelected(PartItem partItem)
    {
        _partItem = partItem;
        if (partItem == null) 
        {
            OnSubGroupSelected(_partsGroup);
            return;
        }
        _panel.SetActive(true);
        _itemNameText.text = partItem.gameObject.name;
        _priceText.text = $"{_pricePrefix}{partItem.Price}{_pricePostFix}";
        UpdateItemCountText();
    }
    
    private void AddItemToCart()
    {
        if (_partItem != null)
        {
            OnAnyAddItemToCart?.Invoke(_partItem);
            UpdateItemCountText();
            return;
        }

        if (_partsGroup != null)
        {
            OnAnyAddSubGroupToCart?.Invoke(_partsGroup);
            UpdateItemCountText();
            return;
        }

        if (_parentGroup != null)
        {
            OnAnyAddGroupToCart?.Invoke(_parentGroup);
            UpdateItemCountText();
            return;
        }
    }

    private void UpdateItemCountText()
    {
        if (_partItem != null)
        {
            _itemCountText.text = CartController.Instance.GetItemCount(_partItem).ToString();
            return;
        }
            
        if (_partsGroup != null)
        {
            _itemCountText.text = CartController.Instance.GetSubGroupCount(_partsGroup).ToString();
            return;
        }
           
        if (_parentGroup != null)
        {
            _itemCountText.text = CartController.Instance.GetGroupCount(_parentGroup).ToString();
            return;
        }
    }
}