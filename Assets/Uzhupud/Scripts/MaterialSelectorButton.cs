using UnityEngine;
using UnityEngine.UI;

public class MaterialSelectorButton : MonoBehaviour
{
    [SerializeField] private Image colorImage;

    int index;
    MaterialSelectorUI selector;

    public void Setup(MaterialSelectorUI selector, int index, Color color)
    {
        this.selector = selector;
        this.index = index;

        if (colorImage != null)
            colorImage.color = color;
    }

    public void SelectIndex()
    {
        if (selector == null) return;
        Debug.Log("Selecting material index: " + index);
        Debug.Log("Selector" + selector);
        selector.SelectIndex(index);
    }
}
