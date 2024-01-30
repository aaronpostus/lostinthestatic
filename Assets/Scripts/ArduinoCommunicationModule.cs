using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class ArduinoCommunicationModule : MonoBehaviour
{
    [SerializeField] Radio radio;
    SerialPort data_stream = new SerialPort("COM3", 9600);
    string receivedBuffer = "";

    // Use a Coroutine for continuous reading
/*    IEnumerator Start()
    {
        try
        {
            data_stream.Open();
            data_stream.ReadTimeout = 25;
            while (true)
            {
                Serial_Data_Reading();
                //yield return new WaitForSeconds(0.1f);
            }
        }
        catch
        {
            Debug.Log("Failed to establish a connection to the Arduino.");
        }
    }*/

    public void SendToLCD(string message)
    {
        data_stream.Write(message + "\n");
    }

    void Serial_Data_Reading()
    {
        if (data_stream.IsOpen)
        {
            // Check if '\n' is present in the received data
            if (receivedBuffer.Contains("\n"))
            {
                // Process the data when '\n' is found
                Debug.Log("Received: " + receivedBuffer);
                ProcessMessage(receivedBuffer);

                // Reset the received data
                receivedBuffer = string.Empty;
            }

            // Accumulate data using ReadExisting
            receivedBuffer += data_stream.ReadExisting();
        }
    }

    void ProcessMessage(string message)
    {
        Debug.Log(message);
        message = message.Replace("?", "");
        string[] input = message.Split(new char[] { ',', '\n' });
        if (input.Length % 7 != 0)
        {
            return;
        }

        radio.TuneRadio(float.Parse(input[input.Length - 7]) / 1000.0f);
        radio.TuneVolume(float.Parse(input[input.Length - 6]) / 1000.0f);

        for (int i = 5; i > 0; i--)
        {
            if (Int32.Parse(input[input.Length - i]) == 0)
            {
                radio.ButtonPress(i - 1);
            }
        }
        receivedBuffer = "";
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