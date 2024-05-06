using DG.Tweening;
using Lean.Common;
using Lean.Touch;
using NaughtyAttributes;
using UnityEngine;

public class CameraTeleportAction : MonoBehaviour
{
    private LeanPitchYaw _leanPitchYaw;
    private LeanMultiUpdate _leanMultiUpdate;
    private LeanPinchCamera _leanPinchCamera;
    private LeanMouseWheel _leanMouseWheel;
    private LeanDragCamera _leanDragCamera;
    private LeanChase _leanChasePivot;

    [SerializeField] private Vector3 _wholeObjectGroupOffset = new Vector3(0f, 0.32f, -2.25f);
    [SerializeField] private Vector3 _parentGroupOffset = new Vector3(0f, 0.4f, -2.25f);
    [SerializeField] private Vector3 _partsGroupOffset = new Vector3(0f, 0.4f, -2.25f);
    [SerializeField] private float _parentGroupZoomLevel = 20f;
    [SerializeField] private float _partsGroupZoomLevel = 10f;

    [BoxGroup("Tweening")] [SerializeField] 
    private Vector3 _cameraOrigin = new Vector3(-0.1f, .4f, -2.25f);

    [BoxGroup("Tweening")] [SerializeField] 
    private float _cameraTransitionSpeed = 2.5f; 

    [BoxGroup("Tweening")] [SerializeField] 
    private Ease _cameraTransitionEase = Ease.OutQuad;

    private ParentGroup _selectedParentGroup;
    private PartsGroup _selectedPartsGroup;
    private Tween _cameraTransitionTween;

    private Machine _machine;
    private Vector3 _machineCenterPosition;
    private Vector3 _offset;
    private Vector3 _cameraStartPosition;

    private float _newZoomLevel;
    private float _defaultZoomLevel;

    private void Awake()
    {
        _leanPitchYaw = GetComponentInParent<LeanPitchYaw>();
        _leanMultiUpdate = GetComponentInParent<LeanMultiUpdate>(); 
        _leanChasePivot = GetComponentInParent<LeanChase>(); 
        _leanPinchCamera = GetComponent<LeanPinchCamera>();
        _leanMouseWheel = GetComponent<LeanMouseWheel>();
        _leanDragCamera = GetComponent<LeanDragCamera>();

        _defaultZoomLevel = _leanPinchCamera.Zoom;
        _newZoomLevel = _defaultZoomLevel;

        _offset = _wholeObjectGroupOffset;
    }

    private void OnEnable()
    {
        Machine.OnAnyInitializeMachine += OnInitializeMachine;
        ParentGroup.OnAnySelected += OnParentGroupSelected;
        PartsGroup.OnAnySelected += OnPartsGroupSelected;

        ParentGroup.OnAnyDeSelected += OnParentGroupDeSelected;
        PartsGroup.OnAnyDeSelected += OnPartsGroupDeSelected;

        PartSelectionController.OnAnyMouseRightClick += UpdateLeanPinchCamera;
    }

    private void OnDisable()
    {
        Machine.OnAnyInitializeMachine -= OnInitializeMachine;
        ParentGroup.OnAnySelected -= OnParentGroupSelected;
        PartsGroup.OnAnySelected -= OnPartsGroupSelected;

        ParentGroup.OnAnyDeSelected -= OnParentGroupDeSelected;
        PartsGroup.OnAnyDeSelected -= OnPartsGroupDeSelected;

        PartSelectionController.OnAnyMouseRightClick -= UpdateLeanPinchCamera;
    }

    private void OnInitializeMachine(Machine machine)
    {
        _machine = machine;
        _machineCenterPosition = _machine.GetBoundsCenterPosition();
        TelePortCameraToPosition(_machineCenterPosition);
    }

    private void OnParentGroupSelected(ParentGroup parentGroup)
    {
        _selectedParentGroup = parentGroup;
        _offset = _parentGroupOffset;
        _newZoomLevel = _parentGroupZoomLevel;
        TelePortCameraToPosition(parentGroup.GetBoundsCenterPosition());
    }

    private void OnPartsGroupSelected(PartsGroup partsGroup)
    {
        _selectedPartsGroup = partsGroup;
        _offset = _partsGroupOffset;
        _newZoomLevel = _partsGroupZoomLevel;
        TelePortCameraToPosition(partsGroup.GetBoundsCenterPosition());
    }

    private void OnParentGroupDeSelected()
    {
        _selectedParentGroup = null;
        _offset = _wholeObjectGroupOffset;
        _newZoomLevel = _defaultZoomLevel;
        TelePortCameraToPosition(_machineCenterPosition);
    }

    private void OnPartsGroupDeSelected()
    {
        _selectedPartsGroup = null;
        _offset = _parentGroupOffset;
        _newZoomLevel = _parentGroupZoomLevel;
        TelePortCameraToPosition(_selectedParentGroup.GetBoundsCenterPosition());
    }

    private void TelePortCameraToPosition(Vector3 position)
    {
        _leanChasePivot.SetPosition(position);

        if (_cameraTransitionTween != null)
            _cameraTransitionTween.Kill();

        _cameraTransitionTween = transform.DOLocalMove(_cameraOrigin, _cameraTransitionSpeed)
        .SetSpeedBased()
        .SetEase(_cameraTransitionEase);

        _offset = _wholeObjectGroupOffset;
        _leanPitchYaw.ResetRotation();
        _leanPinchCamera.Zoom = _newZoomLevel;
        _newZoomLevel = _defaultZoomLevel;
    }

    private void UpdateLeanPinchCamera()
    {
        _leanPinchCamera.enabled = !_leanPinchCamera.enabled;
    }

    private void UpdateLeanScripts(bool enabled)
    {
        _leanPitchYaw.enabled = enabled;
        _leanMultiUpdate.enabled = enabled;
        _leanPinchCamera.enabled = enabled;
        _leanMouseWheel.enabled = enabled;
        _leanDragCamera.enabled = enabled;
    }
}
