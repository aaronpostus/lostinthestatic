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
        eventRef.setParameterByName("SKIP", 0);
    }

    public override void ExitAudioZone()
    {
        eventRef.setParameterByName("SKIP", 1);
    }
}
