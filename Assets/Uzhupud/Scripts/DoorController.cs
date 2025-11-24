using UnityEngine;
using StarterAssets;

public class DoorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private Vector3 localAxis = Vector3.up;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private bool startOpen = false;

    [Header("Interaction")]
    public string playerTag = "Player";
    public GameObject buttonUI;

    public bool IsOpen { get; private set; }

    Quaternion closedRotation;
    Quaternion openRotation;
    Coroutine rotatingCo;

    StarterAssetsInputs inputs;
    bool playerInside = false;

    void Awake()
    {
        closedRotation = transform.localRotation;
        Vector3 axis = localAxis == Vector3.zero ? Vector3.up : localAxis.normalized;
        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, axis);

        if (startOpen)
        {
            transform.localRotation = openRotation;
            IsOpen = true;
        }
        else
        {
            transform.localRotation = closedRotation;
            IsOpen = false;
        }

        if (buttonUI != null)
            buttonUI.SetActive(false);
    }

    void Update()
    {
        if (!playerInside) return;
        if (inputs == null) return;
        if (IsOpen) return;

        if (inputs.interact)
        {
            inputs.interact = false;
            Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInside = true;
        inputs = other.GetComponentInParent<StarterAssetsInputs>();

        if (!IsOpen && buttonUI != null)
            buttonUI.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInside = false;

        if (buttonUI != null)
            buttonUI.SetActive(false);

        inputs = null;
    }

    public void Interact()
    {
        if (IsOpen) return;

        SetOpen(true);

        if (buttonUI != null)
            buttonUI.SetActive(false);

        Collider triggerCol = GetComponent<Collider>();
        if (triggerCol != null)
            triggerCol.enabled = false;

        playerInside = false;
    }

    public void SetOpen(bool open, bool instant = false)
    {
        if (IsOpen == open) return;

        if (rotatingCo != null) StopCoroutine(rotatingCo);

        Quaternion target = open ? openRotation : closedRotation;

        if (instant || duration <= 0f)
        {
            transform.localRotation = target;
            IsOpen = open;
        }
        else
        {
            rotatingCo = StartCoroutine(RotateTo(target, open));
        }
    }

    System.Collections.IEnumerator RotateTo(Quaternion target, bool finalState)
    {
        Quaternion start = transform.localRotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localRotation = Quaternion.Slerp(start, target, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        transform.localRotation = target;
        IsOpen = finalState;
        rotatingCo = null;
    }

    public void InteractFromButton()
    {
        if (!playerInside) return;
        Interact();
    }
}
