using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using static RadioData;

// A dialogue will start from the beginning the first time the user seeks to it.
// Then, while they are selected on that station, it will continue looping.
// If the user seeks away, it will continue to the end of the audio file then wait for the user to seek back before restarting.
// 
// This channel is intended to be used for narrative heavy channels rather than something like music.
public class DialogueChannel : IRadioChannel
{
    EventInstance loopEventInstance;
    private GameObject attentuationObject;
    public DialogueChannel(RadioChannelData radioData, GameObject attentuationObject) { 
        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(radioData.FMODEventRef);
        this.attentuationObject = attentuationObject;
        //this.loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));
    }

    public void SeekTo() {
        PLAYBACK_STATE playbackState;
        loopEventInstance.getPlaybackState(out playbackState);
        loopEventInstance.setVolume(1.0f);
        if (playbackState == PLAYBACK_STATE.STOPPED)
        {
            Debug.Log("Last time the loop stopped! Starting from beginning");
            loopEventInstance.start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopEventInstance, attentuationObject.transform);
        }
        loopEventInstance.setParameterByName("LOOP",1);
        loopEventInstance.getParameterByName("LOOP", out var value);
        Debug.Log("LOOP:"+value);
    }
    public void SeekAwayFrom() {
        loopEventInstance.setVolume(0.0f);
        loopEventInstance.setParameterByName("LOOP", 0f);
    }

    public void InitializeAudioTrigger()
    {

    }
}