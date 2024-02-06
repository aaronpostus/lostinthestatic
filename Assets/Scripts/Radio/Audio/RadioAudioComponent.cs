using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using static RadioData;
public class RadioAudioComponent : MonoBehaviour
{
    [SerializeField] RadioData data;
    private Dictionary<float, IRadioChannel> radioChannels;
    private IRadioChannel currentRadioChannel = null;
    public bool HasEventInstance(float freq) {
        return radioChannels.ContainsKey(freq);
    }
    public EventInstance GetEventInstance(float freq) {
        return radioChannels[freq].GetEventInstance();
    }
    public IRadioChannel GetRadioChannel(float freq) {
        return radioChannels[freq];
    }
    private void CreateChannels() {
        radioChannels = new Dictionary<float, IRadioChannel>();
        foreach (RadioChannelData channelData in data.channels) {
            radioChannels.Add(channelData.channelFrequency, CreateChannel(channelData));
        }
    }
    private IRadioChannel CreateChannel(RadioChannelData channelData) {
        return channelData.channelType switch
        {
            RadioChannelType.MUSIC => new MusicChannel(channelData, this.gameObject),
            RadioChannelType.INVISIBLE_MAZE => new InvisibleMazeChannel(channelData),
            RadioChannelType.DIALOGUE => new DialogueChannel(channelData, this.gameObject),
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
