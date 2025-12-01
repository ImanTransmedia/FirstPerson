using UnityEngine;

public class PlatformObjectActivator : MonoBehaviour
{
    public GameObject target;
    public bool enableOnPC = true;
    public bool enableOnMobile = false;

    void Awake()
    {
        if (target == null)
            target = gameObject;

#if UNITY_ANDROID || UNITY_IOS
        bool isMobile = true;
        bool isPC = false;
#else
        bool isMobile = false;
        bool isPC = true;
#endif

        bool active = (isPC && enableOnPC) || (isMobile && enableOnMobile);
        target.SetActive(active);
    }
}
