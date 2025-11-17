using UnityEngine;
using UnityEngine.Events;

public class PisoUIData
{
    public Texture imagen;
    public string nombre;
    public int level;
    public bool ingresable;
    public string sceneName;
}

public class VRSeleccionManager : MonoBehaviour
{
    public static UnityAction<PisoUIData> OnUIData;


    VRSelectable actual;

    public void Seleccionar(VRSelectable nuevo)
    {
        if (actual == nuevo) return;

        if (actual != null) actual.SetActivo(false);
        actual = nuevo;
        if (actual != null) actual.SetActivo(true);


        if (actual != null)
        {
            OnUIData?.Invoke(new PisoUIData
            {
                imagen = actual.imagenPiso,
                nombre = actual.id,
                level = actual.level,
                ingresable = actual.esIngresable,
                sceneName = actual.sceneName
            });
        }
        else
        {
            OnUIData?.Invoke(null);
        }
    }

    public void Deseleccionar(VRSelectable item)
    {
        if (actual == item)
        {
            actual.SetActivo(false);
            actual = null;
            OnUIData?.Invoke(null);
        }
    }

    public void Regresar()
    {
        if (actual != null) actual.SetActivo(false);
        actual = null;
        OnUIData?.Invoke(null);
    }
}
