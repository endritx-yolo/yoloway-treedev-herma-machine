using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [BoxGroup("Buttons")] [SerializeField] private Button _removeFromCartButton;

    private ParentGroup _parentGroup;
    private PartsGroup _partsGroup;
    private PartItem _partItem;

    public TextMeshProUGUI ItemNameText => _itemNameText; 
    public TextMeshProUGUI PriceText => _priceText; 
    public Button RemoveFromCartButton => _removeFromCartButton;

    public void AddGroupToCart(ParentGroup obj) => _parentGroup = obj;

    public void AddSubGroupToCart(PartsGroup obj) => _partsGroup = obj;

    public void AddItemToCart(PartItem obj) => _partItem = obj;

    public void DisableThisItem() 
    {
        if (_partItem != null)
            CartController.Instance.ItemGroupList.Remove(_partItem);
        else if (_partsGroup != null)
            CartController.Instance.PartsGroupList.Remove(_partsGroup);
        else if (_parentGroup != null)
            CartController.Instance.ParentGroupList.Remove(_parentGroup);

        _partItem = null;
        _partsGroup = null;
        _parentGroup = null;

        gameObject.SetActive(false);
    }
}
