using FMOD.Studio;
using UnityEngine;

public class LoopingChannel : IRadioChannel
{
    EventInstance loopEventInstance;
    public LoopingChannel(string fmodEventDir, Transform transform) { 
        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventDir);
        Update(transform);
    }
    public void SeekTo() {
        loopEventInstance.start();
    }
    public void SeekAwayFrom() {
        loopEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
    public void Update(Transform transform) {
        this.loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    }
}