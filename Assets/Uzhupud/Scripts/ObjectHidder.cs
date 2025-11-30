using UnityEngine;

public class ObjectHidder : MonoBehaviour
{
    public GameObject Target
        ;
    void Start()
    {
        Target.SetActive(false);
    }


}
