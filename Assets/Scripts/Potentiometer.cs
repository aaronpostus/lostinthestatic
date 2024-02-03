using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using Uduino;
using UnityEngine;
using UnityEngine.UI;

public class Potentiometer : MonoBehaviour
{
    [SerializeField] private Slider slider, slider2;
    UduinoManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = UduinoManager.Instance;
        manager.pinMode(AnalogPin.A0, PinMode.Input);
        manager.pinMode(AnalogPin.A1, PinMode.Input);
    }

    // Update is called once per frame
    void Update()
    {
        int analogValue = manager.analogRead(AnalogPin.A0);
        slider.value = (float) analogValue / 1000.0f;
        analogValue = manager.analogRead(AnalogPin.A1);
        slider2.value = (float)analogValue / 1000.0f;
    }

}
