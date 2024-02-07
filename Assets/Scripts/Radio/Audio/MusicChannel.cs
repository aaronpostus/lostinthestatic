using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// A music channel starts from the beginning the first time the player seeks to it.
// It will then repeat forever.
public class MusicChannel : IRadioChannel
{
    EventInstance loopEventInstance;
    public MusicChannel(EventReference fmodEventRef, GameObject attentuationObject) { 
        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventRef);
        this.loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));
    }
    public void SeekTo() {
        loopEventInstance.setVolume(1.0f);
    }
    public void SeekAwayFrom() {
        loopEventInstance.setVolume(0.0f);
    }
}