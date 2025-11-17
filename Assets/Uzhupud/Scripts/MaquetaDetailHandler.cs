using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MaquetaDetailHandler : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public MaquetaOrbitZoom orbit;
    public GameObject infoPanel;
    public Transform detailPosition;

    public Transform cameraPivotNormal;
    public Transform cameraPivotDetail;
    public Transform cameraPivotTop;
    public float cameraLerpSpeed = 5f;

    public float maxClickTime = 0.25f;
    public float maxClickMovement = 10f;

    public UnityEvent onEnterDetailMode;
    public UnityEvent onExitDetailMode;

    bool inDetailMode = false;
    public bool inTopView = false;

    float defaultDistance;
    float originalDistance;

    Vector3 originalPosition;
    Quaternion originalRotation;

    bool mouseDown = false;
    Vector2 mouseDownPos;
    float mouseDownTime;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (target == null) target = transform;
        if (orbit == null) orbit = target.GetComponent<MaquetaOrbitZoom>();
        if (infoPanel != null) infoPanel.SetActive(false);

        originalPosition = target.position;
        originalRotation = target.rotation;

        Transform pivot = GetCurrentPivot();
        float initialDistance = Vector3.Distance(cam.transform.position, pivot.position);
        if (orbit != null) orbit.SetDistance(initialDistance);

        defaultDistance = initialDistance;
        originalDistance = initialDistance;

        UpdateCameraPosition(true);
        UpdateUI();
    }

    void Update()
    {
        HandleClick();
        UpdateCameraPosition(false);
    }

    void HandleClick()
    {
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUI()) return;
                mouseDown = true;
                mouseDownPos = Input.mousePosition;
                mouseDownTime = Time.time;
            }

            if (Input.GetMouseButtonUp(0) && mouseDown)
            {
                mouseDown = false;
                float time = Time.time - mouseDownTime;
                float movement = Vector2.Distance(mouseDownPos, Input.mousePosition);
                bool isClick = time <= maxClickTime && movement <= maxClickMovement;
                if (isClick) TryClick(Input.mousePosition);
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Ended)
            {
                if (IsPointerOverUI()) return;
                if (t.deltaPosition.magnitude <= maxClickMovement && t.deltaTime <= maxClickTime)
                {
                    TryClick(t.position);
                }
            }
        }
    }

    void TryClick(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            bool hitTarget = (hit.transform == target);

            if (!inDetailMode && hitTarget)
            {
                EnterDetailMode();
            }
            else if (inDetailMode && !hitTarget)
            {
                ExitDetailMode();
            }
        }
        else
        {
            if (inDetailMode) ExitDetailMode();
        }
    }

    Transform GetCurrentPivot()
    {
        if (inTopView && cameraPivotTop != null) return cameraPivotTop;
        if (inDetailMode && cameraPivotDetail != null) return cameraPivotDetail;
        if (!inDetailMode && cameraPivotNormal != null) return cameraPivotNormal;
        return target;
    }

    void UpdateCameraPosition(bool instant)
    {
        Transform pivot = GetCurrentPivot();

        float distance = defaultDistance;
        if (orbit != null) distance = orbit.GetCurrentDistance();

        Vector3 desiredPos = pivot.position - pivot.forward * distance;
        Quaternion desiredRot = pivot.rotation;

        if (instant)
        {
            cam.transform.position = desiredPos;
            cam.transform.rotation = desiredRot;
        }
        else
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, desiredPos, cameraLerpSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, desiredRot, cameraLerpSpeed * Time.deltaTime);
        }
    }

    void UpdateUI()
    {
        if (infoPanel != null) infoPanel.SetActive(inDetailMode && !inTopView);
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
        return EventSystem.current.IsPointerOverGameObject();
    }

    void EnterDetailMode()
    {
        inDetailMode = true;
        inTopView = false;

        originalPosition = target.position;
        originalRotation = target.rotation;

        if (orbit != null)
        {
            originalDistance = orbit.GetCurrentDistance();
            orbit.SetControlsEnabled(false);
            orbit.SetDistance(defaultDistance);
        }

        if (detailPosition != null) target.position = detailPosition.position;
        else target.position += new Vector3(1.5f, 0f, 0f);

        Vector3 ang = target.rotation.eulerAngles;
        target.rotation = Quaternion.Euler(0f, ang.y, 0f);

        if (orbit != null) orbit.SetTargetRotation(target.rotation, 0f);

        UpdateUI();
        if (onEnterDetailMode != null) onEnterDetailMode.Invoke();
    }

    public void ExitDetailMode()
    {
        inDetailMode = false;
        inTopView = false;

        target.position = originalPosition;
        target.rotation = originalRotation;

        if (orbit != null)
        {
            orbit.SetDistance(originalDistance);
            orbit.SetTargetRotation(target.rotation, 0f);
            orbit.SetControlsEnabled(true);
        }

        UpdateUI();
        if (onExitDetailMode != null) onExitDetailMode.Invoke();
    }

    public void ToggleTopView()
    {
        if (!inTopView)
        {
            inTopView = true;
            inDetailMode = false;
            if (orbit != null) orbit.SetControlsEnabled(false);
            return;
        }

        inTopView = false;
        inDetailMode = true;
        if (orbit != null) orbit.SetControlsEnabled(false);
    }
}
