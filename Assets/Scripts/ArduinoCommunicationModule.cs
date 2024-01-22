using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class ArduinoCommunicationModule : MonoBehaviour
{
    [SerializeField] Radio radio;
    SerialPort data_stream = new SerialPort("COM3", 9600);
    private string receivedBuffer = "";
    // Start is called before the first frame update
    void Start()
    {
        data_stream.Open();
        InvokeRepeating("Serial_Data_Reading", 0f, 0.1f);
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

            // Check if the buffer contains a complete message
            //int endIndex = receivedBuffer.IndexOf("$");
            //if (endIndex != -1)
           // {
                //string completeMessage = receivedBuffer.Substring(0, endIndex + 1);
                //receivedBuffer = receivedBuffer.Substring(endIndex + 1);

                // Process the complete message
                ProcessMessage(receivedBuffer);
            //}
        }
    }

    void ProcessMessage(string message)
    {
        Debug.Log(message);
        string[] datas = message.Split(",");
        if (datas.Length != 2) {
            return;
        }
        Debug.Log("Tuning to equivalent of input: " + datas[0]);
        radio.TuneRadio(float.Parse(datas[0]) / 1000.0f);
        //radio.TuneVolume(float.Parse(datas[1]) / 1000.0f);
        //slider1.value = (float)Mathf.RoundToInt(float.Parse(datas[0])) / 1000.0f;
        //slider2.value = (float)Mathf.RoundToInt(float.Parse(datas[1])) / 1000.0f;

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
