
using FMOD.Studio;
using UnityEngine;

public abstract class AudioTrigger : MonoBehaviour
{
    private const string playerTag = "Player";

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
    public abstract void AssignEventInstance(EventInstance eventInst);
}
