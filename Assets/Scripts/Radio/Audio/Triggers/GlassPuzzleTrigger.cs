using FMOD.Studio;
using UnityEngine;

public class GlassPuzzleTrigger : AudioTrigger
{
    private EventInstance eventRef;
    public override void AssignEventInstance(EventInstance eventInst)
    {
        eventRef = eventInst;
        Debug.Log("this happened");
    }

    public override void EnterAudioZone(GameObject gameObject)
    {
        Debug.Log(eventRef.isValid());
        Debug.Log(eventRef.setParameterByName("SKIP", 0));
        eventRef.getParameterByName("SKIP", out var value);
        Debug.Log("SKIP:" + value);
    }

    public override void ExitAudioZone()
    {
        Debug.Log(eventRef.setParameterByName("SKIP", 1));
        eventRef.getParameterByName("SKIP", out var value);
        Debug.Log("SKIP:" + value);
    }
}
