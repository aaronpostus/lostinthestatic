using FMOD.Studio;
using UnityEngine;

public interface IRadioChannel
{
    public void SeekTo();
    public void SeekAwayFrom();
    public EventInstance GetEventInstance();
}