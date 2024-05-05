using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIMachineItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<UIMachineItem> OnUIItemHighlighted;

    [SerializeField] 
    private PartItem _partItem;

    private Button _button;    
    private Transform _mask;

    private bool _isSelected;

    public PartItem PartItem => _partItem;
    
    public void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(EnableItem);
        _mask = transform.Find("Mask");
    }

    private void OnEnable()
    {
        if (PartItem == null) return;
        PartItem.OnHighlighted += MarkHighlighted;
        PartItem.OnUnHighlighted += UnMark;
    }

    private void OnDisable()
    {
        if (PartItem == null) return;
        PartItem.OnHighlighted += MarkHighlighted;
        PartItem.OnUnHighlighted += UnMark;
    }

    private void EnableItem()
    {
        _partItem?.Select();
    }

    public void Select()
    {
        _isSelected = true;
        _mask.gameObject.SetActive(true);
        OnUIItemHighlighted?.Invoke(this);
    }

    public void Deselect()
    {
        _isSelected = false;
        if (_mask == null) return;
        _mask.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_partItem == null) return;
        _partItem.Highlight();
        _mask.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (_isSelected) return;
        if (_partItem == null) return;
        _partItem.DeHighlight();
        _mask.gameObject.SetActive(false);
    }

    private void MarkHighlighted()
    {
        _mask.gameObject.SetActive(true);
    }

    private void UnMark()
    {
        if (_isSelected) return;
        _mask.gameObject.SetActive(false);
    }
}
