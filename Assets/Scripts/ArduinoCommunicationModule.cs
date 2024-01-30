using System;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class ArduinoCommunicationModule : MonoBehaviour
{
    [SerializeField] Radio radio;
    SerialPort data_stream = new SerialPort("COM3", 9600);
    string receivedBuffer = "";
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            data_stream.Open();
            data_stream.ReadTimeout = 25;
            InvokeRepeating("Serial_Data_Reading", 0f, 0.1f);
        }
        catch {
            Debug.Log("Failed to establish a connection to the arduino.");
        }
    }
    public void SendToLCD(string message) {
        data_stream.Write(message + "\n");
    }
    // Update is called once per frame
    void Serial_Data_Reading()
    {
        if (data_stream.IsOpen)
        {
            receivedBuffer += data_stream.ReadExisting();
            ProcessMessage(receivedBuffer);
        }
    }
    void ProcessMessage(string message)
    {
        Debug.Log(message);
        message = message.Replace("?", "");
        string[] input = message.Split(new char[] { ',', '\n' });
        if (input.Length % 7 != 0) {
            return;
        }
        radio.TuneRadio(float.Parse(input[input.Length - 7]) / 1000.0f);
        radio.TuneVolume(float.Parse(input[input.Length - 6]) / 1000.0f);
        for (int i = 5; i > 0; i--) {
            if (Int32.Parse(input[input.Length - i]) == 0)
            {
                radio.ButtonPress(i - 1);
            }
        }
        receivedBuffer = "";
        //data_stream.
    }
    private void OnDestroy()
    {
        // Close the serial port when the script is destroyed
        if (data_stream.IsOpen)
        {
            SendToLCD("LCD TURNED OFF");
            data_stream.Close();
        }
    }
}
