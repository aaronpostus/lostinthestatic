using FMOD.Studio;
using UnityEngine;
using static RadioData;
public class InvisibleMazeChannel : IRadioChannel
{
    enum MAZE_STATE { PLAYER_INSIDE, PLAYER_OUTSIDE }
    enum CHANNEL_STATE { LISTENING, NOT_LISTENING }

    EventInstance loopEventInstance;
    private MAZE_STATE state = MAZE_STATE.PLAYER_OUTSIDE;
    private CHANNEL_STATE channelState = CHANNEL_STATE.NOT_LISTENING;
    public InvisibleMazeChannel(RadioChannelData radioData, GameObject attentuationObject)
    {
        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(radioData.FMODEventRef);
        loopEventInstance.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopEventInstance, attentuationObject.transform);
        loopEventInstance.setVolume(0);
        //this.loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attentuationObject));
    }
    public void EnterMaze()
    {
        state = MAZE_STATE.PLAYER_INSIDE;
        if (channelState == CHANNEL_STATE.LISTENING) {
            TurnOnChannelVol();
        }
    }
    public void ExitMaze()
    {
        state = MAZE_STATE.PLAYER_OUTSIDE;
        if (channelState == CHANNEL_STATE.LISTENING) {
            TurnOffChannelVol();
        }
    }
    public EventInstance GetEventInstance()
    {
        return loopEventInstance;
    }
    public void SeekAwayFrom()
    {
        TurnOffChannelVol();
        channelState = CHANNEL_STATE.NOT_LISTENING;
    }
    public void SeekTo()
    {
        if (state == MAZE_STATE.PLAYER_INSIDE) {
            TurnOnChannelVol();
        }
        channelState = CHANNEL_STATE.LISTENING;
    }
    public void UpdateBalance(float balance)
    {
        loopEventInstance.setParameterByName("BLEND", balance);
    }
    private void TurnOnChannelVol() {
        loopEventInstance.setVolume(1f);
    }
    private void TurnOffChannelVol()
    {
        loopEventInstance.setVolume(0);
    }
}