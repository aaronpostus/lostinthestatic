using FMOD.Studio;
using FMODUnity;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using static RadioData;

// A dialogue will start from the beginning the first time the user seeks to it.
// Then, while they are selected on that station, it will continue looping.
// If the user seeks away, it will continue to the end of the audio file then wait for the user to seek back before restarting.
// 
// This channel is intended to be used for narrative heavy channels rather than something like music.
//
public class DialogueChannel : IRadioChannel
{
    private EventInstance loopEventInstance;
    private GameObject attentuationObject;

    public DialogueChannel(RadioChannelData radioData, GameObject attentuationObject) { 
        loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(radioData.FMODEventRef);
        this.attentuationObject = attentuationObject;
    }
    public void SeekTo() {
        PLAYBACK_STATE playbackState;
        loopEventInstance.getPlaybackState(out playbackState);
        loopEventInstance.setVolume(1.0f);
        SubtitleManager.Instance.AddCallback(loopEventInstance);
        if (playbackState == PLAYBACK_STATE.STOPPED)
        {
            loopEventInstance.start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopEventInstance, attentuationObject.transform);
        }
        loopEventInstance.setParameterByName("LOOPDIALOGUE",1);
    }
    public void SeekAwayFrom() {
        loopEventInstance.setVolume(0.0f);
        loopEventInstance.setParameterByName("LOOPDIALOGUE", 0f);
        SubtitleManager.Instance.ClearSubtitles();
        SubtitleManager.Instance.RemoveCallback(loopEventInstance);
    }

    public EventInstance GetEventInstance()
    {
        return loopEventInstance;
    }
}