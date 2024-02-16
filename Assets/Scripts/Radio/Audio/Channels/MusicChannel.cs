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
        Debug.Log(attentuationObject.transform.GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopEventInstance, attentuationObject.transform, attentuationObject.transform.GetComponent<Rigidbody>());
        loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));
        loopEventInstance.setVolume(0.0f);
    }
    public void SeekTo() {
        loopEventInstance.setVolume(1.0f);
    }
    public void SeekAwayFrom() {
        loopEventInstance.setVolume(0.0f);
    }
    public EventInstance GetEventInstance()
    {
        return loopEventInstance;
    }
}