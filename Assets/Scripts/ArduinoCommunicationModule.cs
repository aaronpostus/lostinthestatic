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
        data_stream.Open();
        data_stream.ReadTimeout = 25;
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
            ProcessMessage(receivedBuffer);
        }
    }
    void ProcessMessage(string message)
    {
        Debug.Log(message);
        message = message.Replace("?", "");
        string[] datas = message.Split(new char[] { ',', '\n' });
        if (datas.Length % 7 != 0) {
            return;
        }
        //Debug.Log("Tuning to equivalent of input: " + datas[datas.Length-7]);
        radio.TuneRadio(float.Parse(datas[datas.Length - 7]) / 1000.0f);
        //radio.TuneVolume(float.Parse(datas[1]) / 1000.0f);
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
