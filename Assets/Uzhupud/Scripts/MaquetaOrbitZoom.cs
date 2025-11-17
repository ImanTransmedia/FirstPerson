using UnityEngine;
using UnityEngine.EventSystems;

public class MaquetaOrbitZoom : MonoBehaviour
{
    public Transform target;
    public Camera cam;

    public float rotationSpeedMouse = 150f;
    public float rotationSpeedTouch = 5f;
    public float xMinLimit = -60f;
    public float xMaxLimit = 60f;

    public float minDistance = 2f;
    public float maxDistance = 8f;
    public float zoomSpeedMouse = 3f;
    public float zoomSpeedTouch = 0.02f;

    public float rotationLerpSpeed = 10f;

    bool controlsEnabled = true;

    float currentDistance;

    float pitchAngle = 0f;

    float lastPinchDistance = 0f;

    Quaternion targetRotation;

    public float GetCurrentDistance()
    {
        return currentDistance;
    }

    public void SetDistance(float d)
    {
        currentDistance = Mathf.Clamp(d, minDistance, maxDistance);
    }

    public void SetControlsEnabled(bool e)
    {
        controlsEnabled = e;
    }

    public void SetTargetRotation(Quaternion rot, float newPitch)
    {
        targetRotation = rot;
        pitchAngle = newPitch;
    }

    void Awake()
    {
        if (target == null) target = transform;
        if (cam == null) cam = Camera.main;
        targetRotation = target.rotation;
        currentDistance = (minDistance + maxDistance) * 0.5f;
    }

    void Update()
    {
        if (controlsEnabled)
        {
            HandleRotation();
            HandleZoom();
        }

        target.rotation = Quaternion.Slerp(
            target.rotation,
            targetRotation,
            rotationLerpSpeed * Time.deltaTime
        );
    }

    void HandleRotation()
    {
        if (IsPointerOverUI()) return;

        float dx = 0f;
        float dy = 0f;
        float speed = 0f;
        bool hasInput = false;

        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButton(0))
            {
                dx = Input.GetAxis("Mouse X");
                dy = Input.GetAxis("Mouse Y");
                speed = rotationSpeedMouse;
                hasInput = true;
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                dx = t.deltaPosition.x;
                dy = t.deltaPosition.y;
                speed = rotationSpeedTouch;
                hasInput = true;
            }
        }

        if (!hasInput) return;

        dx *= speed * Time.deltaTime;
        dy *= speed * Time.deltaTime;

        Vector3 camUp = cam.transform.up;
        Vector3 camRight = cam.transform.right;

        Quaternion yaw = Quaternion.AngleAxis(dx, camUp);

        float newPitch = Mathf.Clamp(pitchAngle - dy, xMinLimit, xMaxLimit);
        float deltaPitch = newPitch - pitchAngle;
        pitchAngle = newPitch;

        Quaternion pitch = Quaternion.AngleAxis(deltaPitch, camRight);

        targetRotation = yaw * pitch * targetRotation;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            currentDistance -= scroll * zoomSpeedMouse;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }

        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float d = Vector2.Distance(t0.position, t1.position);

            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                lastPinchDistance = d;
            }
            else
            {
                float delta = d - lastPinchDistance;
                lastPinchDistance = d;

                currentDistance -= delta * zoomSpeedTouch;
                currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
            }
        }
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
        return EventSystem.current.IsPointerOverGameObject();
    }
}
