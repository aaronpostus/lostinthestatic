using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    [SerializeField] ArduinoCommunicationModule arduino;
    [SerializeField] Slider volumeSlider, frequencySlider;
    public const int maxRadioFreq = 103, minRadioFreq = 93, minVol = 0, maxVol = 38;
    private int freqRange, volRange;
    private float frequency = 100, volume = 15;
    public Radio() {
        freqRange = maxRadioFreq - minRadioFreq;
        volRange = (maxVol - minVol) + 1;
    }
    // volume range is 0f -> 1f
    public void TuneVolume(float value)
    {
        // Ensure the input value is between 0 and 1
        value = Mathf.Max(0, Mathf.Min(1, value));
        // Map the input value to the integer range
        int result = minVol + (int)(value * volRange);
        SetVolume(result);
    }
    // tune to an appropriate station from input between 0f - 1f
    public void TuneRadio(float value)
    {
        // Ensure the input value is between 0 and 1
        value = Mathf.Max(0, Mathf.Min(1, value));
        // Map the input value to the output range
        float freq = minRadioFreq + value * freqRange;
        SetFrequency(freq);
    }
    // set radio to an exact frequency (i.e. 99.5f)
    private void SetFrequency(float freq) {
        // don't do anything if the frequency hasn't actually changed
        if (this.frequency == freq) {
            return;
        }
        this.frequency = freq;
        RefreshFrequencyLCD();
    }
    // set radio to an exact frequency (i.e. 99.5f)
    private void SetVolume(float vol)
    {
        // don't do anything if the volume hasn't actually changed
        if (this.volume == vol) {
            return;
        }
        this.volume = vol;

    }
    // updates the controller LCD
    private void RefreshFrequencyLCD() {
        arduino.SendToLCD(frequency + "");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
