using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class UICartButtonPresenter : MonoBehaviour
{
    [SerializeField] private GameObject _itemCountGobj;
    [SerializeField] private TextMeshProUGUI _itemCountText;
    [SerializeField] private GameObject _cartPanel;
    [SerializeField] private TextMeshProUGUI _totalPriceText;

    [BoxGroup("Buttons")] [SerializeField] private Button _cartPanelButton;
    [BoxGroup("Buttons")] [SerializeField] private Button _cartPanelBackButton;

    [SerializeField] private UIItemCard[] _itemCardArray;

    private int _itemCardIndex = 0;
    private int _itemCount = 0;

    private readonly string _totalPricePrefix = "Total Price: ";
    private readonly string _pricePrefix = "Price: ";
    private readonly string _pricePostFix = " Euro";

    public int ItemCount
    {
        get => _itemCount;
        set
        {
            _itemCount = value;
            _itemCountGobj.SetActive(_itemCount > 0);
            _itemCountText.text = _itemCount.ToString();
        }
    }

    private void Awake()
    {
        for (int i = 0; i < _itemCardArray.Length; i++)
        {
            _itemCardArray[i].gameObject.SetActive(false);
            _itemCardArray[i].RemoveFromCartButton.onClick.AddListener(RemoveItemFromCard);
        }

        _cartPanel.SetActive(false);

        _cartPanelButton.onClick.AddListener(EnableCartPanel);
        _cartPanelBackButton.onClick.AddListener(DisableCartPanel);
    }

    private void OnEnable()
    {
        CartController.OnAnyAddNewItemToCard += UpdateItemCount;
        UIMachinePresenter.OnAnyAddGroupToCart += OnAddGroupToCart;
        UIMachinePresenter.OnAnyAddSubGroupToCart += OnAddSubGroupToCart;
        UIMachinePresenter.OnAnyAddItemToCart += OnAddItemToCart;
    }

    private void OnDisable()
    {
        CartController.OnAnyAddNewItemToCard -= UpdateItemCount;
        UIMachinePresenter.OnAnyAddGroupToCart -= OnAddGroupToCart;
        UIMachinePresenter.OnAnyAddSubGroupToCart -= OnAddSubGroupToCart;
        UIMachinePresenter.OnAnyAddItemToCart -= OnAddItemToCart;
    }

    private void UpdateItemCount()
    {
        _itemCardArray[ItemCount].gameObject.SetActive(true);
        ItemCount++;
        UpdateTotalCartPrice();
    }

    private void RemoveItemFromCard()
    {
        ItemCount--;
        _itemCardIndex--;
        _itemCardArray[_itemCardIndex].DisableThisItem();
        UpdateTotalCartPrice();
    }

    private void OnAddGroupToCart(ParentGroup obj)
    {
        _itemCardArray[_itemCardIndex].ItemNameText.text = obj.name;
        _itemCardArray[_itemCardIndex].PriceText.text = $"{_pricePrefix}{obj.Price}{_pricePostFix}";
        _itemCardArray[_itemCardIndex].AddGroupToCart(obj);
        _itemCardIndex++;
    }

    private void OnAddSubGroupToCart(PartsGroup obj)
    {
        _itemCardArray[_itemCardIndex].ItemNameText.text = obj.name;
        _itemCardArray[_itemCardIndex].PriceText.text = $"{_pricePrefix}{obj.Price}{_pricePostFix}";
        _itemCardArray[_itemCardIndex].AddSubGroupToCart(obj);
        _itemCardIndex++;
    }

    private void OnAddItemToCart(PartItem obj)
    {
        _itemCardArray[_itemCardIndex].ItemNameText.text = obj.name;
        _itemCardArray[_itemCardIndex].PriceText.text = $"{_pricePrefix}{obj.Price}{_pricePostFix}";
        _itemCardArray[_itemCardIndex].AddItemToCart(obj);
        _itemCardIndex++;
    }

    private void UpdateTotalCartPrice()
    {
        int totalPrice = 0;
        for (int i = 0; i < CartController.Instance.ParentGroupList.Count; i++)
            totalPrice += CartController.Instance.ParentGroupList[i].Price;
        for (int i = 0; i < CartController.Instance.PartsGroupList.Count; i++)
            totalPrice += CartController.Instance.PartsGroupList[i].Price;
        for (int i = 0; i < CartController.Instance.ItemGroupList.Count; i++)
            totalPrice += CartController.Instance.ItemGroupList[i].Price;

        _totalPriceText.text = $"{_totalPricePrefix}{totalPrice}{_pricePostFix}";
    }

    private void EnableCartPanel() => _cartPanel.SetActive(true);
    private void DisableCartPanel() => _cartPanel.SetActive(false);
}