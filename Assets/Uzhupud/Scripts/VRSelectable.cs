using UnityEngine;

public class VRSelectable : MonoBehaviour
{
    public string id = "Piso_0";
    public GameObject highlight;
    public Texture imagenPiso;
    public int level;
    public bool esIngresable;
    public string sceneName;
    public bool isSelected = false;


    public void TogleSelected()
    {
        if (isSelected)
        {
            Deseleccionar();
            isSelected = false;
        }
        else
        {
            Seleccionar();
            isSelected = true;
        }
    }

    public void Seleccionar()
    {
        var m = FindFirstObjectByType<VRSeleccionManager>();
        if (m != null) m.Seleccionar(this);
    }

    public void Deseleccionar()
    {
        var m = FindFirstObjectByType<VRSeleccionManager>();
        if (m != null) m.Deseleccionar(this);
        isSelected = false;
    }

    public void SetActivo(bool activo)
    {
        if (highlight != null) highlight.SetActive(activo);
    }

}
