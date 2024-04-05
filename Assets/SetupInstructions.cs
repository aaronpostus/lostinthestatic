using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupInstructions : MonoBehaviour
{
    [SerializeField] GameObject physicalInstructions, normalInstructions;
    [SerializeField] Serial serial;
    void Start()
    {
        if (Serial.usingPhysical)
        {
            normalInstructions.SetActive(false);
        }
        else { 
            physicalInstructions.SetActive(false);
        }
    }
}
