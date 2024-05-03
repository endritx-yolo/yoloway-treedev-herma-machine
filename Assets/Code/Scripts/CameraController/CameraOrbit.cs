using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public GameObject _parentModel;

    private float _rotationSpeed = 50f;
    private Vector3 _mouseWorldPosStart;
    private float _zoomScale = 10f;
    private float _maxFOV = 160f;
    private float _minFOV = 0f;
    private float _defFOV = 60f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse2))
            CamOrbit();

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.F))
            FitToScreen();

        if (Input.GetMouseButtonDown(2) && !Input.GetKey(KeyCode.LeftShift))
            _mouseWorldPosStart = GetPerspectivePos();

        if (Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftShift))
            Pan();

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void CamOrbit()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, -verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }
    }

    private Bounds GetBound(GameObject parentGameObj)
    {
        Bounds bound = new Bounds(parentGameObj.transform.position, Vector3.zero);
        var rList = parentGameObj.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer r in rList)
            bound.Encapsulate(r.bounds);

        return bound;
    }
    
    public void FitToScreen()
    {
        Camera.main.fieldOfView = _defFOV;
        Bounds bound = GetBound(_parentModel);
        Vector3 boundSize = bound.size;
        float boundDiagonal =
            Mathf.Sqrt((boundSize.x * boundSize.x) + (boundSize.y * boundSize.y) + (boundSize.z * boundSize.z));
        float camDistanceToBoundCentre = boundDiagonal / 2f / (Mathf.Tan(Camera.main.fieldOfView / 2f * Mathf.Deg2Rad));
        float camDistanceBoundWithOffset = camDistanceToBoundCentre + boundDiagonal / 2f -
                                           (Camera.main.transform.position - transform.position).magnitude;
        transform.position = bound.center + (-transform.forward * camDistanceBoundWithOffset);
    }

    private void Pan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 mouseWorldPossDiff = _mouseWorldPosStart - GetPerspectivePos();
            transform.position += mouseWorldPossDiff;
        }
    }

    private void Zoom(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            _mouseWorldPosStart = GetPerspectivePos();
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoomDiff * _zoomScale, _minFOV, _maxFOV);
            Vector3 mouseWorldPosDiff = _mouseWorldPosStart - GetPerspectivePos();
        }
    }
    
    public Vector3 GetPerspectivePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(transform.forward, 0f);
        float dist;
        plane.Raycast(ray, out dist);
        return ray.GetPoint(dist);
    }
}