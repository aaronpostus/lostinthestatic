using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using static RadioData;

// A music channel starts from the beginning when the game starts. It will then repeat forever.
public class MusicChannel : IRadioChannel
{
    EventInstance loopEventInstance;
    public MusicChannel(RadioChannelData radioData, GameObject attentuationObject)
    {
        loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(radioData.FMODEventRef);
        loopEventInstance.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopEventInstance, attentuationObject.transform);
        loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));
        if (radioData.hasAudioTrigger)
        {
            // NOT NOT GOOD BAD BAD DO DIFFERENT LATER TECH DEBT TECH DEBT
            AudioTrigger trigger = GameObject.Find("GlassPuzzleAudioTrigger").GetComponent<GlassPuzzleTrigger>();
            trigger.AssignEventInstance(loopEventInstance);
        }
        loopEventInstance.setVolume(0.0f);

    }
    public void SeekTo() {
        loopEventInstance.setVolume(1.0f);
    }
    public void SeekAwayFrom() {
        loopEventInstance.setVolume(0.0f);
    }
}