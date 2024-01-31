using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Serial))]
public class ArduinoCommunicationModule : MonoBehaviour
{
    [SerializeField] Radio radio;
    void OnSerialLine(string line)
    {
        if (!Serial.usingPhysical) return;
        Debug.Log(line);
        string[] input = line.Split(new char[] { ',', '\n' });
        if (input.Length % 7 != 0)
        {
            return;
        }
        float radioFreq, radioVol;
        int[] buttons = new int[5];
        try
        {
            radioFreq = float.Parse(input[input.Length - 7]) / 1000.0f;
            radioVol = float.Parse(input[input.Length - 6]) / 1000.0f;
            for (int i = 0; i < 5; i++)
            {
                buttons[i] = Int32.Parse(input[i+2]);
            }
        }
        catch {
            Debug.Log("Received invalid communication.");
            return;
        }
        radio.TuneRadio(radioFreq);
        radio.TuneVolume(radioVol);
        for (int i = 0; i < 5; i++) {
            radio.ButtonInteraction(i, buttons[i] == 0);
        }

    }
}