using FMOD.Studio;
using UnityEngine;

public class GlassPuzzleTrigger : AudioTrigger
{
    private EventInstance eventRef;
    public override void AssignEventInstance(EventInstance eventInst)
    {
        eventRef = eventInst;
    }

    public override void EnterAudioZone(GameObject gameObject)
    {
        Debug.Log(eventRef.setParameterByName("SKIP", 0));
        Debug.Log("Enter");
    }

    public override void ExitAudioZone()
    {
        Debug.Log(eventRef.setParameterByName("SKIP", 1));
        Debug.Log("Exit");
    }
}
