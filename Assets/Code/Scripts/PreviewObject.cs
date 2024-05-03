using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private Vector3 _previousPosition;
    private Vector3 _positionDelta;

    private Camera _mainCamera;
    private Transform _cachedCameraTransform;
    private Transform _cachedTransform;

    [SerializeField] private float _rotationSpeed = 1f;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _cachedTransform = transform;
        _cachedCameraTransform = _mainCamera.transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _previousPosition = Vector3.zero;
            return;
        }
        
        if (Input.GetMouseButton(0))
        {
            float rotationSpeed = _rotationSpeed * Time.deltaTime;
            _positionDelta = Input.mousePosition - _previousPosition;

            if (Vector3.Dot(_cachedTransform.up, Vector3.up) >= 0)
                _cachedTransform.Rotate(transform.up,
                    -Vector3.Dot(_positionDelta, _cachedCameraTransform.right) * rotationSpeed,
                    Space.World);
            else
                _cachedTransform.Rotate(transform.up,
                    Vector3.Dot(_positionDelta, _cachedCameraTransform.right) * rotationSpeed,
                    Space.World);

            _cachedTransform.Rotate(_cachedCameraTransform.right,
                Vector3.Dot(_positionDelta, _cachedCameraTransform.up) * rotationSpeed, Space.World);
        }

        _previousPosition = Input.mousePosition;
    }
}