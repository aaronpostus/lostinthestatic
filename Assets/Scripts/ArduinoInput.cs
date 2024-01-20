using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class ArduinoInput : MonoBehaviour
{
    [SerializeField] Slider slider1, slider2;
    SerialPort data_stream = new SerialPort("COM3", 9600);
    List<string> testMessages = new List<string>() { "Message One!", "Message Two!", "Message Three!" };
    public string receivedstring;
    private string receivedBuffer = "";
    // Start is called before the first frame update
    void Start()
    {
        data_stream.Open();
        InvokeRepeating("Serial_Data_Reading", 0f, 0.1f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            string message = "#" + testMessages[Random.Range(0, testMessages.Count)] + "$";
            data_stream.Write(message);
        }
    }

    // Update is called once per frame
    void Serial_Data_Reading()
    {
        if (data_stream.IsOpen)
        {
            receivedBuffer += data_stream.ReadExisting();

            // Check if the buffer contains a complete message
            int endIndex = receivedBuffer.IndexOf("$");
            if (endIndex != -1)
            {
                string completeMessage = receivedBuffer.Substring(0, endIndex + 1);
                receivedBuffer = receivedBuffer.Substring(endIndex + 1);

                // Process the complete message
                ProcessMessage(completeMessage);
            }
        }
    }

    void ProcessMessage(string message)
    {
        string[] datas = message.Split(",");
        slider1.value = (float)Mathf.RoundToInt(float.Parse(datas[0])) / 1000.0f;
        slider2.value = (float)Mathf.RoundToInt(float.Parse(datas[1])) / 1000.0f;
        Debug.Log(message);
    }

    private void OnDestroy()
    {
        // Close the serial port when the script is destroyed
        if (data_stream.IsOpen)
        {
            data_stream.Close();
        }
    }
}
