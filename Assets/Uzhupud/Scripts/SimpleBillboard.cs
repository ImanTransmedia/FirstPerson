using UnityEngine;

public class SimpleBillboard : MonoBehaviour
{
    public Camera cam;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 direccion = cam.transform.position - transform.position;
        direccion.y = 0f;

        if (direccion.sqrMagnitude < 0.0001f) return;

        Quaternion rotacion = Quaternion.LookRotation(direccion);
        transform.rotation = rotacion;
    }
}
