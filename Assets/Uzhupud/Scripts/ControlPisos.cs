using System;
using UnityEngine;

public class ControlPisos : MonoBehaviour
{
    public GameObject[] pisos;
    public MaquetaDetailHandler detalleMaqueta;
    public Transform maqueta;
    public Vector3 eje = Vector3.up;

    Transform pivot;

    void Awake()
    {
        CrearPivotCentro();
    }

    void CrearPivotCentro()
    {
        if (maqueta == null) return;

        Renderer[] rs = maqueta.GetComponentsInChildren<Renderer>();
        if (rs.Length == 0) return;

        Bounds b = rs[0].bounds;
        for (int i = 1; i < rs.Length; i++)
            b.Encapsulate(rs[i].bounds);

        Vector3 centro = b.center;

        GameObject go = new GameObject("PivotMaqueta");
        Transform t = go.transform;
        t.position = centro;
        t.rotation = maqueta.rotation;
        t.localScale = Vector3.one;

        t.parent = maqueta.parent;
        maqueta.parent = t;

        pivot = t;
    }

    public void CambiarPiso(float valor)
    {
        int indice = Mathf.RoundToInt(valor);

        for (int i = 0; i < pisos.Length; i++)
        {
            pisos[i].SetActive(i <= indice);
            if (i == indice && detalleMaqueta.inTopView)
                pisos[i].GetComponent<VRSelectable>().Seleccionar();
        }
    }

    public void Rotar(float valor)
    {
        Transform t = pivot != null ? pivot : maqueta;
        if (t != null)
            t.localRotation = Quaternion.Euler(eje * valor);
    }

    public void Escalar(float valor)
    {
        Transform t = pivot != null ? pivot : maqueta;
        if (t != null)
            t.localScale = Vector3.one * valor;
    }
}
