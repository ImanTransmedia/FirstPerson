using UnityEngine;
using UnityEngine.EventSystems;

public class FloorPicker : MonoBehaviour
{
    public Camera cam;
    public LayerMask selectableMask = ~0;
    public float maxTapTime = 0.25f;
    public float maxTapMove = 12f;

    bool press;
    float t0;
    Vector2 p0;

    void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                press = true;
                t0 = Time.time;
                p0 = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0) && press && !IsPointerOverUI())
            {
                press = false;
                if (IsTap(p0, Input.mousePosition, t0, Time.time))
                    TryPick(Input.mousePosition);
            }
        }
        else if (Input.touchCount == 1)
        {
            var t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began && !IsPointerOverUI())
            {
                press = true;
                t0 = Time.time;
                p0 = t.position;
            }
            if (t.phase == TouchPhase.Ended && press && !IsPointerOverUI())
            {
                press = false;
                if (IsTap(p0, t.position, t0, Time.time))
                    TryPick(t.position);
            }
        }
        else
        {
            press = false;
        }
    }

    bool IsTap(Vector2 a, Vector2 b, float ta, float tb)
    {
        return (tb - ta) <= maxTapTime && Vector2.Distance(a, b) <= maxTapMove;
    }

    void TryPick(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit, 1000f, selectableMask))
        {
            var sel = hit.collider.GetComponent<VRSelectable>();
            Debug.Log(sel.name);
            if (sel != null) sel.Seleccionar();
        }
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
        return EventSystem.current.IsPointerOverGameObject();
    }
}
