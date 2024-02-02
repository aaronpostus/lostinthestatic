﻿using System;
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
        public string fmodEventDir;
        public RadioChannelType channelType;
    }

    public List<RadioChannelData> channels;
}