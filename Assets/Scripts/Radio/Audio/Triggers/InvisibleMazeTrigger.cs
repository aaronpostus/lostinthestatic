using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class InvisibleMazeTrigger : AudioTrigger
{
    private EventInstance eventRef;
    private GameObject playerGameObj;
    [SerializeField] RadioAudioComponent audioComponent;
    [SerializeField] float channelNumber;
    public void Start()
    {
        this.eventRef = audioComponent.GetEventInstance(channelNumber);
    }
    public override void EnterAudioZone(GameObject gameObject)
    {
        this.playerGameObj = gameObject;
        eventRef.setVolume(1);
    }

    public override void ExitAudioZone()
    {
        eventRef.setVolume(0);
    }
}
