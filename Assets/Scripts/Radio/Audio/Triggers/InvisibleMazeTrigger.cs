using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class InvisibleMazeTrigger : AudioTrigger
{
    private EventInstance eventRef;
    private GameObject playerGameObj;
    public override void AssignEventInstance(EventInstance eventInst)
    {
        eventRef = eventInst;
    }
    public override void EnterAudioZone(GameObject gameObject)
    {
        this.playerGameObj = gameObject;
    }

    public override void ExitAudioZone()
    {

    }
}
