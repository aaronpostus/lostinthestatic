using FMOD.Studio;
using FMODUnity;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using static SubtitleManager;
using System.Diagnostics;
public class Ending1 : MonoBehaviour
{
    public float rotationSpeed = 30f; // Speed of rotation in degrees per second
    private TimelineInfo timelineInfo;
    private GCHandle timelineHandle;
    private FMOD.Studio.EVENT_CALLBACK beatCallback;
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        PLAYBACK_STATE playbackState;
        loopEventInstance.getPlaybackState(out playbackState);
        if (playbackState == PLAYBACK_STATE.STOPPED) {
            Application.Quit();
            UnityEngine.Debug.Log("WEee");
        }
    }
    void Start()
    {
        // Rotate the GameObject around its up axis (Y axis) continuously


        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(endingAudio);

        loopEventInstance.setVolume(1.0f);
        SubtitleManager.Instance.ClearSubtitles();
        loopEventInstance.start();

        SubtitleManager.Instance.AddCallback(loopEventInstance);
    }

    public EventReference endingAudio;
    private FMOD.Studio.EventInstance loopEventInstance;

}