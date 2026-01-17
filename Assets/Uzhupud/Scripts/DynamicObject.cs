using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class DynamicObject : MonoBehaviour
{
    public List<Material> materials = new List<Material>();
    public MeshRenderer[] targetRenderers;
    public int defaultIndex = 0;
    public string playerTag = "Player";
    public GameObject buttonUI;

    MaterialSelectorUI selectorUI;
    StarterAssetsInputs inputs;

    int currentIndex = -1;
    bool playerInside = false;

    void Awake()
    {
        if (selectorUI == null)
            selectorUI = FindObjectOfType<MaterialSelectorUI>();
    }

    void OnEnable()
    {
        ApplyMaterial(defaultIndex);
    }

    void Update()
    {
        if (!playerInside) return;
        if (inputs == null) return;

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
        if (buttonUI != null)
            buttonUI.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInside = false;

        if (selectorUI == null)
            selectorUI = FindObjectOfType<MaterialSelectorUI>();

        if (selectorUI != null)
            selectorUI.Close();

        if (buttonUI != null)
            buttonUI.SetActive(false);

        inputs = null;
    }

    public void ApplyMaterial(int index)
    {
        if (materials == null || materials.Count == 0) return;
        if (index < 0 || index >= materials.Count) index = 0;

        currentIndex = index;
        Material m = materials[currentIndex];
        if (m == null || targetRenderers == null) return;

        for (int i = 0; i < targetRenderers.Length; i++)
        {
            var mr = targetRenderers[i];
            if (mr == null) continue;

            if (Application.isPlaying)
                mr.material = m;
            else
                mr.sharedMaterial = m;
        }
    }


    public void Interact()
    {
        if (selectorUI == null)
            selectorUI = FindObjectOfType<MaterialSelectorUI>();

        if (selectorUI == null) return;
        selectorUI.OpenFor(this);

        if (buttonUI != null)
            buttonUI.SetActive(false);
    }

    public void ShowButtonUI()
    {
        if (buttonUI != null)
            buttonUI.SetActive(true);
    }

    public void InteractFromButton()
    {
        if (!playerInside) return;
        Interact();
    }

    public List<Material> GetMaterials()
    {
        return materials;
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}
