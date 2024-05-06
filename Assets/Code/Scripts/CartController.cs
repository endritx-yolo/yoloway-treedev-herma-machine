using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leon.Singleton;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public static event Action OnAnyAddNewItemToCard;

    [SerializeField] private List<ParentGroup> _parentGroupList = new List<ParentGroup>();
    [SerializeField] private List<PartsGroup> _partsGroupList = new List<PartsGroup>();
    [SerializeField] private List<PartItem> _itemGroupList = new List<PartItem>();

    #region Properties

    public List<ParentGroup> ParentGroupList => _parentGroupList;
    public List<PartsGroup> PartsGroupList => _partsGroupList;
    public List<PartItem> ItemGroupList => _itemGroupList;

    #endregion

    private void OnEnable()
    {
        UIMachinePresenter.OnAnyAddGroupToCart += OnAddGroupToCart;
        UIMachinePresenter.OnAnyAddSubGroupToCart += OnAddSubGroupToCart;
        UIMachinePresenter.OnAnyAddItemToCart += OnAddItemToCart;
    }

    private void OnDisable()
    {
        UIMachinePresenter.OnAnyAddGroupToCart -= OnAddGroupToCart;
        UIMachinePresenter.OnAnyAddSubGroupToCart -= OnAddSubGroupToCart;
        UIMachinePresenter.OnAnyAddItemToCart -= OnAddItemToCart;
    }

    private void OnAddGroupToCart(ParentGroup parentGroup)
    {
        _parentGroupList.Add(parentGroup);
        OnAnyAddNewItemToCard?.Invoke();
    }

    private void OnAddSubGroupToCart(PartsGroup partsGroup)
    {
        _partsGroupList.Add(partsGroup);
        OnAnyAddNewItemToCard?.Invoke();
    }

    private void OnAddItemToCart(PartItem partItem)
    {
        _itemGroupList.Add(partItem);
        OnAnyAddNewItemToCard?.Invoke();
    }

    public int GetGroupCount(ParentGroup parentGroup)
    {
        int instanceId = parentGroup.GetInstanceID();
        return _parentGroupList.Count(x => x.GetInstanceID() == instanceId);
    }

    public int GetSubGroupCount(PartsGroup partsGroup)
    {
        int instanceId = partsGroup.GetInstanceID();
        return _partsGroupList.Count(x => x.GetInstanceID() == instanceId);
    }

    public int GetItemCount(PartItem partItem)
    {
        int instanceId = partItem.GetInstanceID();
        return _itemGroupList.Count(x => x.GetInstanceID() == instanceId);
    }
}
