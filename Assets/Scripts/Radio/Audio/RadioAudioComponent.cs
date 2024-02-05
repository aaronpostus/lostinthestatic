using FMOD.Studio;
using System.Collections.Generic;

using UnityEngine;
using static RadioData;

public class RadioAudioComponent : MonoBehaviour
{
    [SerializeField] RadioData data;
    private Dictionary<float, IRadioChannel> radioChannels;
    private IRadioChannel currentRadioChannel = null;
    private void CreateChannels() {
        this.radioChannels = new Dictionary<float, IRadioChannel>();
        foreach (RadioChannelData channelData in data.channels) {
            radioChannels.Add(channelData.channelFrequency, CreateChannel(channelData));
        }
    }
    private IRadioChannel CreateChannel(RadioChannelData channelData) {
        return channelData.channelType switch
        {
            // change later to include classes for the other types of channels
            RadioChannelType.STANDARD => new LoopingChannel(channelData.FMODEventRef, gameObject),
            RadioChannelType.INVISIBLE_MAZE => new LoopingChannel(channelData.FMODEventRef, gameObject),
            RadioChannelType.LOOPING => new LoopingChannel(channelData.FMODEventRef, gameObject),
            _ => throw new System.NotImplementedException(),
        };
    }

    public void Seek(float radioChannel) {
        FMODUnity.RuntimeManager.PlayOneShot(data.radioSeekNoise, transform.position);
        if (this.radioChannels == null) {
            CreateChannels();
        }
        if (this.currentRadioChannel != null) {
            currentRadioChannel.SeekAwayFrom();
        }
        if (this.radioChannels.ContainsKey(radioChannel)) {
            this.currentRadioChannel = this.radioChannels[radioChannel];
            this.currentRadioChannel.SeekTo();
        }
        else {
            this.currentRadioChannel = null;
            // static time woooooooooooooooooooo
        }
    }
}
