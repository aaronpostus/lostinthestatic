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
            IRadioChannel channel;
            switch (channelData.channelType) {
                // change later to include classes for the other types of channels
                case RadioChannelType.LOOPING:
                    channel = new LoopingChannel(channelData.fmodEventDir, this.transform);
                    break;
                case RadioChannelType.STANDARD:
                    channel = new LoopingChannel(channelData.fmodEventDir, this.transform);
                    break;
                case RadioChannelType.INVISIBLE_MAZE:
                    channel = new LoopingChannel(channelData.fmodEventDir, this.transform);
                    break;
                default:
                    channel = new LoopingChannel(channelData.fmodEventDir, this.transform);
                    break;
            }
            radioChannels.Add(channelData.channelFrequency, channel);
        }
    }
    private void Update()
    {
        if (currentRadioChannel == null) {
            return;
        }
        currentRadioChannel.Update(this.transform);
    }
    public void Seek(float radioChannel) {
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
