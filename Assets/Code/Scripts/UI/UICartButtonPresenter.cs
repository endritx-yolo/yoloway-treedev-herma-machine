using TMPro;
using UnityEngine;

public class UICartButtonPresenter : MonoBehaviour
{
    [SerializeField] private GameObject _itemCountGobj;
    [SerializeField] private TextMeshProUGUI _itemCountText;

    private int _itemCount = 0;

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

    private void OnEnable()
    {
        CartController.OnAnyAddNewItemToCard += UpdateItemCount;
    }

    private void OnDisable()
    {
        CartController.OnAnyAddNewItemToCard -= UpdateItemCount;
    }

    private void UpdateItemCount()
    {
        ItemCount++;
    }
}
