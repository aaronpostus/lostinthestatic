using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// A looping channel will start from the beginning the first time the user seeks to it.
// Then, while they are selected on that station, it will continue looping.
// If the user seeks away, it will continue to the end of the audio file then wait for the user to seek back before restarting.
// 
// This channel is intended to be used for narrative heavy channels rather than something like music.
public class LoopingChannel : IRadioChannel
{
    EventInstance loopEventInstance;
    public LoopingChannel(EventReference fmodEventRef, GameObject attentuationObject) { 
        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventRef);
        this.loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));

    }
    public void SeekTo() {
        PLAYBACK_STATE playbackState;
        loopEventInstance.getPlaybackState(out playbackState);
        loopEventInstance.setVolume(1.0f);
        if (playbackState == PLAYBACK_STATE.STOPPED)
        {
            Debug.Log("Last time the loop stopped! Starting from beginning");
            loopEventInstance.start();
        }
        loopEventInstance.setParameterByName("LOOP",1);
        loopEventInstance.getParameterByName("LOOP", out var value);
        Debug.Log("LOOP:"+value);
    }
    public void SeekAwayFrom() {
        loopEventInstance.setVolume(0.0f);
        loopEventInstance.setParameterByName("LOOP", 0f);
    }
}