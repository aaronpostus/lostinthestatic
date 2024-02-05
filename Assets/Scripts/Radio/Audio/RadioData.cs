using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RadioData : ScriptableObject
{
    public enum RadioChannelType { LOOPING, STANDARD, INVISIBLE_MAZE }

    [Serializable]
    public class RadioChannelData
    {
        public string channelName;
        public float channelFrequency;
        public EventReference FMODEventRef;
        public RadioChannelType channelType;
    }

    public List<RadioChannelData> channels;
    public EventReference radioSeekNoise;
}
