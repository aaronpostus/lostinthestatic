using FMODUnity;
using UnityEngine;

public class InvisibleMazeTrigger : AudioTrigger
{
    private EventReference fmodEventRef;
    private GameObject playerGameObj;
    public InvisibleMazeTrigger(string playerTag, EventReference fmodEventRef) : base(playerTag) {
        this.fmodEventRef = fmodEventRef;
    }

    public override void EnterAudioZone(GameObject gameObject)
    {
        this.playerGameObj = gameObject;
    }

    public override void ExitAudioZone()
    {
        throw new System.NotImplementedException();
    }
}
