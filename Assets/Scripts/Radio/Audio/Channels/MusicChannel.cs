using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using static RadioData;

// A music channel starts from the beginning the first time the player seeks to it.
// It will then repeat forever.
public class MusicChannel : IRadioChannel
{
    EventInstance loopEventInstance;
    public MusicChannel(RadioChannelData radioData, GameObject attentuationObject)
    {
        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(radioData.FMODEventRef);
        this.loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));
    }

    public void SeekTo() {
        loopEventInstance.setVolume(1.0f);
    }
    public void SeekAwayFrom() {
        loopEventInstance.setVolume(0.0f);
    }

    public void InitializeAudioTrigger()
    {
        
    }
}