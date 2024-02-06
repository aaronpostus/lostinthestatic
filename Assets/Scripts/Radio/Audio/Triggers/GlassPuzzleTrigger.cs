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
        eventRef.setParameterByName("LOOP", 0);
    }

    public override void ExitAudioZone()
    {
        eventRef.setParameterByName("LOOP", 1);
    }
}
