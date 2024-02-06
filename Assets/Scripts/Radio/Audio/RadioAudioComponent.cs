using FMOD.Studio;
using System.Collections.Generic;

using UnityEngine;
using static RadioData;

public class RadioAudioComponent : MonoBehaviour
{
    [SerializeField] RadioData data;
    [SerializeField] Collider playerCollider;
    private Dictionary<float, IRadioChannel> radioChannels;
    private IRadioChannel currentRadioChannel = null;
    private void CreateChannels() {
        radioChannels = new Dictionary<float, IRadioChannel>();
        foreach (RadioChannelData channelData in data.channels) {
            radioChannels.Add(channelData.channelFrequency, CreateChannel(channelData));
        }
    }
    private IRadioChannel CreateChannel(RadioChannelData channelData) {
        return channelData.channelType switch
        {
            // change later to include classes for the other types of channels
            RadioChannelType.MUSIC => new MusicChannel(channelData, gameObject),
            RadioChannelType.INVISIBLE_MAZE => new DialogueChannel(channelData, gameObject),
            RadioChannelType.DIALOGUE => new DialogueChannel(channelData, gameObject),
            _ => throw new System.NotImplementedException(),
        };
    }

    public void Seek(float radioChannel) {
        FMODUnity.RuntimeManager.PlayOneShot(data.radioSeekNoise, transform.position);
        if (radioChannels == null) {
            CreateChannels();
        }
        currentRadioChannel?.SeekAwayFrom();
        if (radioChannels.ContainsKey(radioChannel)) {
            currentRadioChannel = this.radioChannels[radioChannel];
            currentRadioChannel.SeekTo();
        }
        else {
            this.currentRadioChannel = null;
            // static time woooooooooooooooooooo
        }
    }
}
