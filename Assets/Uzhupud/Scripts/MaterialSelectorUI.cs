using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class MaterialSelectorUI : MonoBehaviour
{
    public GameObject root;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private MaterialSelectorButton buttonPrefab;

    public List<Material> materials = new List<Material>();

    DynamicObject current;
    StarterAssetsInputs inputs;

    readonly List<MaterialSelectorButton> spawnedButtons = new List<MaterialSelectorButton>();
    bool isOpen = false;

    void Awake()
    {
        if (root != null) root.SetActive(false);

        if (inputs == null)
            inputs = FindObjectOfType<StarterAssetsInputs>();
    }

    public void OpenFor(DynamicObject target)
    {
        current = target;

        materials.Clear();
        ClearButtons();

        if (inputs == null)
            inputs = FindObjectOfType<StarterAssetsInputs>();

        if (inputs != null)
        {
            inputs.cursorLocked = false;
            inputs.cursorInputForLook = false;
            inputs.SetCursorState(false);
            inputs.move = Vector2.zero;
            inputs.look = Vector2.zero;
            inputs.jump = false;
            inputs.sprint = false;
        }

        if (current != null && current.materials != null && current.materials.Count > 0)
        {
            materials.AddRange(current.materials);

            for (int i = 0; i < materials.Count; i++)
            {
                Material mat = materials[i];
                if (mat == null) continue;

                MaterialSelectorButton btn = Instantiate(buttonPrefab, buttonsParent);
                Color baseColor = GetBaseColor(mat);

                btn.Setup(this, i, baseColor);
                spawnedButtons.Add(btn);
            }
        }

        if (root != null) root.SetActive(true);
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen) return;

        if (root != null) root.SetActive(false);

        if (inputs == null)
            inputs = FindObjectOfType<StarterAssetsInputs>();

        if (inputs != null)
        {
            inputs.cursorLocked = true;
            inputs.cursorInputForLook = true;
            inputs.SetCursorState(true);
        }
        current.ShowButtonUI();

        current = null;
        materials.Clear();
        ClearButtons();
        isOpen = false;
    }

    public void Toggle()
    {
        if (root == null) return;
        bool value = !root.activeSelf;
        if (value)
            OpenFor(current);
        else
            Close();
    }

    public void SelectIndex(int index)
    {
        if (current == null) return;
        current.ApplyMaterial(index);
    }

    Color GetBaseColor(Material mat)
    {
        if (mat == null) return Color.white;
        if (mat.HasProperty("_BaseColor"))
            return mat.GetColor("_BaseColor");
        if (mat.HasProperty("_Color"))
            return mat.GetColor("_Color");
        return Color.white;
    }

    void ClearButtons()
    {
        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            if (spawnedButtons[i] != null)
                Destroy(spawnedButtons[i].gameObject);
        }

        spawnedButtons.Clear();
    }
}
