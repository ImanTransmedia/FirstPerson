using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BolaSelectable : MonoBehaviour
{
    public int Indice { get; private set; }
    public Material MaterialBase { get; private set; }

    private DynamicObject _manager;
    private Renderer _rend;
    private Material _highlightMat;
    private Material _cachedBaseMat;

    [Header("Trigger VR")]
    public bool usarTriggers = true;
    public string tagMano = "VRHand";
    public LayerMask capasMano = ~0;

    Collider col;

    // llamado por DynamicObject al crear
    public void Configurar(DynamicObject manager, int indice, Renderer rend, Material baseMat, Material highlightMat)
    {
        _manager = manager;
        Indice = indice;
        _rend = rend;
        MaterialBase = baseMat;
        _highlightMat = highlightMat;
        _cachedBaseMat = baseMat;

        if (_rend != null && baseMat != null)
        {
            if (Application.isPlaying) _rend.material = baseMat;
            else _rend.sharedMaterial = baseMat;
        }

        col = GetComponent<Collider>();
        if (usarTriggers && col != null) col.isTrigger = true;
    }

    public void SetHighlight(bool on)
    {
        if (_rend == null) return;
        if (on && _highlightMat != null)
        {
            if (Application.isPlaying) _rend.material = _highlightMat;
            else _rend.sharedMaterial = _highlightMat;
        }
        else
        {
            if (Application.isPlaying) _rend.material = _cachedBaseMat;
            else _rend.sharedMaterial = _cachedBaseMat;
        }
    }

    public void Seleccionar()
    {
        if (_manager != null) _manager.Seleccionar(this);
    }

    public void Deseleccionar()
    {
        if (_manager != null) _manager.Deseleccionar(this);
    }

    public void OnBoolChanged(bool v)
    {
        if (v) Seleccionar();
        else Deseleccionar();
    }

    public void OnWhenSelect() { Seleccionar(); }
    public void OnWhenUnselect() { Deseleccionar(); }
    public void OnXRISelectEntered(object _) { Seleccionar(); }
    public void OnXRISelectExited(object _) { Deseleccionar(); }

    void OnTriggerEnter(Collider other)
    {
        if (!usarTriggers) return;
        if (CumpleFiltro(other)) Seleccionar();
    }

    void OnTriggerExit(Collider other)
    {
        if (!usarTriggers) return;
        if (CumpleFiltro(other)) Deseleccionar();
    }

    bool CumpleFiltro(Collider other)
    {
        if (!string.IsNullOrEmpty(tagMano) && other.CompareTag(tagMano)) return true;
        return ((1 << other.gameObject.layer) & capasMano) != 0;
    }
}
