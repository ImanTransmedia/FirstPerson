using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DynamicObject : MonoBehaviour
{
    [Header("Datos")]
    public List<Material> materiales = new List<Material>();
    public Material materialSeleccion;           
    public MeshRenderer meshObjetivo;            

    [Header("Instanciacion")]
    public GameObject prefabBola;                
    public Transform ancla;                      
    public Vector3 direccion = Vector3.right;    
    public float espacio = 0.25f;                
    public Vector3 escalaBola = Vector3.one * 0.05f;
    public Quaternion rotacionBola = Quaternion.identity;

    [Header("Editor")]
    public bool instanciarEnEditor = true;      
    public bool dibujarGizmos = true;

    private readonly List<BolaSelectable> _bolas = new List<BolaSelectable>();
    private BolaSelectable _actual;
    private Transform _contenedor;

    void OnEnable()
    {
        if (!Application.isPlaying && instanciarEnEditor) Reconstruir();
    }

    void Start()
    {
        if (Application.isPlaying) Reconstruir();
    }

    void OnValidate()
    {
        direccion = (direccion == Vector3.zero) ? Vector3.right : direccion;
        if (!Application.isPlaying && instanciarEnEditor) Reconstruir();
    }

    public void Reconstruir()
    {
        if (_contenedor == null)
        {
            var t = transform.Find("Bolas");
            _contenedor = t != null ? t : new GameObject("Bolas").transform;
            _contenedor.SetParent(transform, false);
        }

        // limpiar anteriores
        for (int i = _contenedor.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying) Destroy(_contenedor.GetChild(i).gameObject);
            else DestroyImmediate(_contenedor.GetChild(i).gameObject);
        }
        _bolas.Clear();
        _actual = null;

        if (materiales == null) return;

        var origen = ancla ? ancla.position : transform.position;
        var dir = direccion.normalized;

        for (int i = 0; i < materiales.Count; i++)
        {
            var pos = origen + dir * (espacio * i);
            GameObject go;

            if (prefabBola != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    go = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefabBola, _contenedor);
                else
                    go = Instantiate(prefabBola, pos, rotacionBola, _contenedor);
#else
            go = Instantiate(prefabBola, pos, rotacionBola, _contenedor);
#endif
                go.transform.position = pos;
                go.transform.rotation = rotacionBola;
            }
            else
            {
                go = Application.isPlaying
                    ? GameObject.CreatePrimitive(PrimitiveType.Sphere)
                    : GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.name = "Bola_" + i;
                go.transform.SetParent(_contenedor, true);
                go.transform.position = pos;
                go.transform.rotation = rotacionBola;
                var col = go.GetComponent<Collider>();
                if (col != null) col.isTrigger = true;
            }

            go.transform.localScale = escalaBola;

            var rend = go.GetComponentInChildren<Renderer>();
            if (rend == null) rend = go.AddComponent<MeshRenderer>();
            if (materiales[i] != null) rend.sharedMaterial = materiales[i];

            var sel = go.GetComponent<BolaSelectable>();
            if (sel == null) sel = go.AddComponent<BolaSelectable>();

            sel.Configurar(this, i, rend, materiales[i], materialSeleccion);
            _bolas.Add(sel);
        }
    }


    public void Seleccionar(BolaSelectable nuevo)
    {
        if (_actual == nuevo) return;

        if (_actual != null) _actual.SetHighlight(false);
        _actual = nuevo;
        if (_actual != null) _actual.SetHighlight(true);

        if (meshObjetivo != null && nuevo != null && nuevo.MaterialBase != null)
        {
            if (Application.isPlaying)
                meshObjetivo.material = nuevo.MaterialBase;    
            else
                meshObjetivo.sharedMaterial = nuevo.MaterialBase; 
        }
    }

    public void Deseleccionar(BolaSelectable item)
    {
        if (_actual == item)
        {
            _actual.SetHighlight(false);
            _actual = null;
        }
    }

    void OnDrawGizmos()
    {
        if (!dibujarGizmos || materiales == null) return;
        var origen = ancla ? ancla.position : transform.position;
        var dir = direccion == Vector3.zero ? Vector3.right : direccion.normalized;

        Gizmos.matrix = Matrix4x4.identity;
        for (int i = 0; i < materiales.Count; i++)
        {
            var p = origen + dir * (espacio * i);
            Gizmos.DrawWireSphere(p, Mathf.Max(escalaBola.x, Mathf.Max(escalaBola.y, escalaBola.z)) * 0.6f);
        }
    }
}
