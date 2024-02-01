using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RadioAudioComponent
{
    private Dictionary<float, IRadioChannel> radioChannels;
    private float currentRadioChannel;
    public RadioAudioComponent() {
        radioChannels.Add(99.5f, new LoopingChannel());
    }
    public void Seek(float radioChannel) {
        if (radioChannels.ContainsKey(currentRadioChannel)) {
            radioChannels[currentRadioChannel].SeekAwayFrom();
        }
        this.currentRadioChannel = radioChannel;
        if (radioChannels.ContainsKey(currentRadioChannel))
        {
            radioChannels[currentRadioChannel].SeekTo();
        }
        else { 
            // static time wooooooooooooooooooooooo
        }
    }
}
