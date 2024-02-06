using UnityEngine;

public abstract class AudioTrigger : MonoBehaviour
{
    private string playerTag;
    public AudioTrigger(string playerTag) { 
        this.playerTag = playerTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag) {
            EnterAudioZone(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == playerTag)
        {
            ExitAudioZone();
        }
    }
    public abstract void EnterAudioZone(GameObject gameObject);
    public abstract void ExitAudioZone();
}
