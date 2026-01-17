using UnityEngine;
using UnityEngine.Events;

public class TouchHandler : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    public string interactTag = "Interactuable";
    public float maxDistance = 100f;
    public Camera cam;

    public UnityEvent onInteractTap;
    public GameObjectEvent onInteractTapObject;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                HandleTap(t.position);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            HandleTap(Input.mousePosition);
        }
    }

    void HandleTap(Vector2 screenPosition)
    {
        if (cam == null)
            return;

        Ray ray = cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider != null && hit.collider.CompareTag(interactTag))
            {
                if (onInteractTap != null)
                    onInteractTap.Invoke();

                if (onInteractTapObject != null)
                    onInteractTapObject.Invoke(hit.collider.gameObject);
            }
        }
    }
}
