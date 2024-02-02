using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RadioAudioComponent : MonoBehaviour
{
    private Dictionary<float, IRadioChannel> radioChannels;
    private float currentRadioChannel;

    //[SerializeField] FMODUnity.EventReference oneShotEventPath;
    //EventInstance loopEventInstance;

    void Awake() {
        this.radioChannels = new Dictionary<float, IRadioChannel>();
        radioChannels.Add(99.5f, new LoopingChannel());
        /**loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(oneShotEventPath);
        loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        loopEventInstance.start(); **/
    }
    private void Update()
    {
        //loopEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
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
