using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuIngresoController : MonoBehaviour
{
    public RawImage imgPiso;
    public TMP_Text txtLevel;
    public GameObject botonObj;

    string escenaObjetivo;

    void OnEnable()
    {
        VRSeleccionManager.OnUIData += OnData;
    }

    void OnDisable()
    {
        VRSeleccionManager.OnUIData -= OnData;
    }

    void OnData(PisoUIData data)
    {
        if (data == null)
        {
            if (imgPiso != null) imgPiso.texture = null;
            if (txtLevel != null) txtLevel.text = "";
            if (botonObj != null) botonObj.SetActive(false);
            escenaObjetivo = "";
            return;
        }

        if (imgPiso != null) imgPiso.texture = data.imagen;
        if (txtLevel != null) txtLevel.text = "Nivel " + data.level;

        if (botonObj != null) botonObj.SetActive(data.ingresable);
        escenaObjetivo = data.ingresable ? data.sceneName : "";
    }

    public void CargarEscena()
    {
        if (!string.IsNullOrEmpty(escenaObjetivo))
            SceneManager.LoadScene(escenaObjetivo);
    }
}
