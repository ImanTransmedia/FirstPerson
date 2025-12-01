using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

public class CanvaOptionsController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject returnPanel;
    public GameObject returnButton;
    public GameObject interactPanel;

    [Header("Input")]
    public StarterAssetsInputs starterAssetsInputs;

    [Header("Events")]
    public UnityEvent onYesOption;
    public UnityEvent onNoOption;

    bool isOpen;

    void Awake()
    {
        if (starterAssetsInputs == null)
        {
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        }

        if (returnPanel != null)
            returnPanel.SetActive(false);

#if UNITY_ANDROID || UNITY_IOS
        if (returnButton != null)
            returnButton.SetActive(true);
        if (interactPanel != null)
            interactPanel.SetActive(false);
#else
        if (returnButton != null)
            returnButton.SetActive(false);
        if (interactPanel != null)
            interactPanel.SetActive(true);
#endif

        isOpen = false;
    }

    void Update()
    {
        if (starterAssetsInputs == null)
        {
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        }
        if (starterAssetsInputs == null)
            return;

        if (starterAssetsInputs.returnMenu)
        {
            starterAssetsInputs.returnMenu = false;

            if (!isOpen)
                OpenOptions();
            else
                CloseOptions();
        }
    }

    public void OpenOptions()
    {
        if (isOpen) return;

        isOpen = true;

        starterAssetsInputs.move = Vector2.zero;
        starterAssetsInputs.look = Vector2.zero;

        if (returnPanel != null)
            returnPanel.SetActive(true);

        if (returnButton != null)
            returnButton.SetActive(false);
        if (interactPanel != null)
            interactPanel.SetActive(false);

        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.SetCursorForUI(true);
        }
    }

    public void CloseOptions()
    {
        if (!isOpen) return;

        isOpen = false;

        if (returnPanel != null)
            returnPanel.SetActive(false);

#if UNITY_ANDROID || UNITY_IOS
        if (returnButton != null)
            returnButton.SetActive(true);
        if (interactPanel != null && starterAssetsInputs.interact)
            interactPanel.SetActive(false);
#else
        if (returnButton != null)
            returnButton.SetActive(false);
        if (interactPanel != null && starterAssetsInputs.interact)
            interactPanel.SetActive(true);
#endif

        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.SetCursorForUI(false);
        }
    }

    private void InvokeYesOption()
    {
        if (onYesOption != null)
            onYesOption.Invoke();
    }

    public void OpenOptionsFromButton()
    {
        OpenOptions();
    }

    public void CloseOptionsFromButton() { 
        CloseOptions();
}

public void YesOptionFromButton()
{
    InvokeYesOption();
}
}
