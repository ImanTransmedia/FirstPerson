using TMPro;
using UnityEngine;

public class ControlDiaNoche : MonoBehaviour
{
    public Material materialDia;
    public Material materialNoche;
    public Renderer objetoRenderer;
    public bool usarSkybox = true;
    public bool iniciarComoDia = true;
    public Light luzPrincipal;
    public Color colorLuzDia = Color.white;
    public Color colorLuzNoche = new Color(0.1f, 0.1f, 0.3f);
    public float intensidadDia = 1.2f;
    public float intensidadNoche = 0.3f;

    [Header("Control de texto e iconos")]
    [SerializeField] private TextMeshProUGUI textoModo;
    [SerializeField] private GameObject iconoDia;
    [SerializeField] private GameObject iconoNoche;


    bool esDiaActual;

    void Start()
    {
        esDiaActual = iniciarComoDia;
        AplicarModo(esDiaActual);
    }

    void Update()
    {
        bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (ctrl && Input.GetKeyDown(KeyCode.D))
        {
            esDiaActual = !esDiaActual;
            AplicarModo(esDiaActual);
            Debug.Log("Modo: " + (esDiaActual ? "Día" : "Noche"));
        }

        // Actualizar texto e iconos
        if (textoModo != null)
        {
            textoModo.text = esDiaActual ? "Día" : "Noche";
            iconoDia.SetActive(esDiaActual);
            iconoNoche.SetActive(!esDiaActual);
        }
    }

    public void SetModo(bool esDia)
    {
        esDiaActual = esDia;
        AplicarModo(esDiaActual);
    }

    public void SetDia()
    {
        esDiaActual = true;
        AplicarModo(true);
    }

    public void SetNoche()
    {
        esDiaActual = false;
        AplicarModo(false);
    }

    void AplicarModo(bool esDia)
    {
        if (esDia)
        {
            if (usarSkybox && materialDia != null)
                RenderSettings.skybox = materialDia;
            if (objetoRenderer != null && materialDia != null)
                objetoRenderer.material = materialDia;
            if (luzPrincipal != null)
            {
                luzPrincipal.color = colorLuzDia;
                luzPrincipal.intensity = intensidadDia;
            }
        }
        else
        {
            if (usarSkybox && materialNoche != null)
                RenderSettings.skybox = materialNoche;
            if (objetoRenderer != null && materialNoche != null)
                objetoRenderer.material = materialNoche;
            if (luzPrincipal != null)
            {
                luzPrincipal.color = colorLuzNoche;
                luzPrincipal.intensity = intensidadNoche;
            }
        }
    }
}
