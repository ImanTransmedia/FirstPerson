using UnityEngine;

public class MaquetaDetailHandler : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public GameObject infoPanel;

    public Transform cameraPivotNormal;
    public Transform cameraPivotTop;

    public float cameraLerpSpeed = 5f;

    public float normalFocalLength = 60f;
    public float topFocalLength = 120f;
    public float focalLerpSpeed = 5f;

    public bool inTopView = false;

    float defaultDistance;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (target == null) target = transform;

        if (cam != null)
        {
            cam.usePhysicalProperties = true;
            if (normalFocalLength <= 0f) normalFocalLength = cam.focalLength;
            cam.focalLength = normalFocalLength;
        }

        Transform pivot = GetCurrentPivot();
        if (cam != null && pivot != null)
        {
            float initialDistance = Vector3.Distance(cam.transform.position, pivot.position);
            defaultDistance = initialDistance;
        }

        UpdateCameraPosition(true);
        UpdateUI();
    }

    void Update()
    {
        UpdateCameraPosition(false);
    }

    Transform GetCurrentPivot()
    {
        if (inTopView && cameraPivotTop != null) return cameraPivotTop;
        if (cameraPivotNormal != null) return cameraPivotNormal;
        return target;
    }

    void UpdateCameraPosition(bool instant)
    {
        if (cam == null) return;

        Transform pivot = GetCurrentPivot();
        if (pivot == null) return;

        float distance = defaultDistance;
        if (distance <= 0f) distance = 10f;

        Vector3 desiredPos = pivot.position;
        Quaternion desiredRot = pivot.rotation;

        float desiredFocal = inTopView ? topFocalLength : normalFocalLength;

        if (instant)
        {
            cam.transform.position = desiredPos;
            cam.transform.rotation = desiredRot;
            cam.focalLength = desiredFocal;
        }
        else
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, desiredPos, cameraLerpSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, desiredRot, cameraLerpSpeed * Time.deltaTime);
            cam.focalLength = Mathf.Lerp(cam.focalLength, desiredFocal, focalLerpSpeed * Time.deltaTime);
        }
    }

    void UpdateUI()
    {
        if (infoPanel != null) infoPanel.SetActive(!inTopView);
    }

    public void ToggleTopView()
    {
        inTopView = !inTopView;
        //UpdateUI();
        UpdateCameraPosition(false);
    }
}
