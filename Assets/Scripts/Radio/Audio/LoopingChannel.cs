using FMOD.Studio;
using UnityEngine;

public class LoopingChannel : IRadioChannel
{
    EventInstance loopEventInstance;
    public LoopingChannel(string fmodEventDir, GameObject attentuationObject) { 
        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventDir);
        this.loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));
    }
    public void SeekTo() {
        Debug.Log("Seek to looping channel");
        loopEventInstance.start();
    }
    public void SeekAwayFrom() {
        //loopEventInstance.
        Debug.Log("Seek away from looping channel");
        loopEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
    public void Update()
    {
        // Check if the sound has reached the end
        if (IsSoundPlaying() && !IsSoundPlayingInLoop())
        {
            // Restart the sound to loop it
            loopEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            loopEventInstance.start();
        }
    }

    bool IsSoundPlaying()
    {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        loopEventInstance.getPlaybackState(out playbackState);
        return playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING;
    }

    bool IsSoundPlayingInLoop()
    {
        //bool isLooping;
        //loopEventInstance.get3DAttributes(out isLooping);
        // return isLooping;
        return true;
    }

}