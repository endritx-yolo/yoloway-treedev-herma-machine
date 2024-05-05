using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIMachineGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<GameObject> OnAnySelectUIGroup;


    [SerializeField]
    private GameObject _subgroup;

    [SerializeField] private ParentGroup _parentGroup;

    public GameObject Subgroup => _subgroup;

    private Button _button;
    private Transform _mask;

    public ParentGroup ParentGroup => _parentGroup;

    public void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(EnableGroup);
        _mask = transform.Find("Mask");
    }

    private void OnEnable()
    {
        if (ParentGroup == null) return;
        ParentGroup.OnHighlighted += MarkHighlighted;
        ParentGroup.OnUnHighlighted += UnMark;
    }

    private void OnDisable()
    {
        if (ParentGroup == null) return;
        ParentGroup.OnHighlighted += MarkHighlighted;
        ParentGroup.OnUnHighlighted += UnMark;
    }

    private void EnableGroup()
    {
        _subgroup.gameObject.SetActive(true);
        _parentGroup?.Select();
        OnAnySelectUIGroup?.Invoke(_subgroup);
        _parentGroup.DeHighlight();
        _mask.gameObject.SetActive(false);
    }

    public void EnableGroupFrom3DInput()
    {
        _subgroup.gameObject.SetActive(true);
        OnAnySelectUIGroup?.Invoke(_subgroup);
        _mask.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (_parentGroup == null) return;
        _parentGroup.Highlight();
        _mask.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (_parentGroup == null) return;
        _parentGroup.DeHighlight();
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
