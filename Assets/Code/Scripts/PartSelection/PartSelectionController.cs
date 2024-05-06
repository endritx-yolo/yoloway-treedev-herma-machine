using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PartSelectionController : MonoBehaviour
{
    public static event Action OnAnyDeselectItem;
    public static event Action OnAnyMouseRightClick;

    private Camera             _mainCamera;

    [SerializeField] private LayerMask _layerMask;

    private float _mouseHoldDownDuration = 0.2f;
    private float _timer                 = 0.2f;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _timer      = _mouseHoldDownDuration;
    }

    private void Update()
    {
        HandlePartsSelection();
        //HandlePartsDeselection();
    }

    private bool _allowObjectSelection;
    
    private void HandlePartsSelection()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            _allowObjectSelection = true;
            _timer = _mouseHoldDownDuration;
        }

        if (Input.GetMouseButton(0))
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                    _allowObjectSelection = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!_allowObjectSelection) return;

            RaycastHit hitInfo;
            Ray        ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 100.0f, _layerMask))
                if (hitInfo.collider.TryGetComponent(out ISelectableItem item)) { item.Select(); }
        }
    }

    private void HandlePartsDeselection()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnAnyDeselectItem?.Invoke();
    }
}