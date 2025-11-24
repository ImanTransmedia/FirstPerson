using UnityEngine;
using UnityEngine.Events;

public class DoorEventHandler : MonoBehaviour
{
       public UnityEvent OnDoorTap;


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("DoorEventHandler: OnTriggerEnter detected with " + other.name);
        OnDoorTap?.Invoke();    
    }

}
