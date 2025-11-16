using UnityEngine;

public class ControlTransform : MonoBehaviour
{
    public Transform objeto;
    public Vector3 eje = Vector3.up;

    public void Rotar(float valor)
    {
        if (objeto != null)
            objeto.localRotation = Quaternion.Euler(eje * valor);
    }

    public void Escalar(float valor)
    {
        if (objeto != null)
            objeto.localScale = Vector3.one * valor;
    }
}
