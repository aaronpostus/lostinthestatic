using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using static RadioData;

// A music channel starts from the beginning when the game starts. It will then repeat forever.
public class MusicChannel : IRadioChannel
{
    EventInstance loopEventInstance;
    RadioChannelData radioData;
    bool started = false;
    private GameObject attentuationObject;
    public MusicChannel(RadioChannelData radioData, GameObject attentuationObject)
    {
        loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(radioData.FMODEventRef);
        this.attentuationObject = attentuationObject;
        this.radioData = radioData;

    }
    public void SeekTo() {
        if (!started) {
            loopEventInstance.start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopEventInstance, attentuationObject.transform);
            loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));
            if (radioData.trigger)
            {
                radioData.trigger.AssignEventInstance(loopEventInstance);
            }
            started = true;
        }
        loopEventInstance.setVolume(1.0f);
    }
    public void SeekAwayFrom() {
        loopEventInstance.setVolume(0.0f);
    }
}