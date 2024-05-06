using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIMachineSubGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<GameObject> OnAnySelectUISubGroup;

    [SerializeField]
    private GameObject _item;

    [SerializeField] 
    private PartsGroup _partsGroup;

    public GameObject Item => _item;

    private Button _button;
    private Transform _mask;

    public PartsGroup PartsGroup => _partsGroup;

    public void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(EnableSubGroup);
        _mask = transform.Find("Mask");
    }

    private void OnEnable()
    {
        if (PartsGroup == null) return;
        PartsGroup.OnHighlighted += MarkHighlighted;
        PartsGroup.OnUnHighlighted += UnMark;
    }

    private void OnDisable()
    {
        if (PartsGroup == null) return;
        PartsGroup.OnHighlighted -= MarkHighlighted;
        PartsGroup.OnUnHighlighted -= UnMark;
    }

    private void EnableSubGroup()
    {
        _item.gameObject.SetActive(true);
        _partsGroup?.Select();
        OnAnySelectUISubGroup?.Invoke(_item);
        _partsGroup.DeHighlight();
        _mask.gameObject.SetActive(false);
    }

    public void EnableSubGroupFrom3DInput()
    {
        _item.gameObject.SetActive(true);
        OnAnySelectUISubGroup?.Invoke(_item);
        _mask.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_partsGroup == null) return;
        _partsGroup.Highlight();
        _mask.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (_partsGroup == null) return;
        _partsGroup.DeHighlight();
        _mask.gameObject.SetActive(false);
    }

    private void MarkHighlighted()
    {
        _mask.gameObject.SetActive(true);
    }

    private void UnMark()
    {
        _mask.gameObject.SetActive(false);
    }
}
