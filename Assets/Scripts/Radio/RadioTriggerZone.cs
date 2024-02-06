using UnityEngine;
public class RadioTriggerZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) { 
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Player") { 
        
        }
    }
}
