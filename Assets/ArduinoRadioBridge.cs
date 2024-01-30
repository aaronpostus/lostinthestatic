
using UnityEngine;

[RequireComponent(typeof(Serial))]
public class ArduinoRadioBridge : MonoBehaviour
{
    void OnSerialLine(string line) { 
        Debug.Log(line);
    }
}
