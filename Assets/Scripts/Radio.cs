using TMPro;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [Tooltip("Attach the module that feeds the radio input from an arduino. If no module is attached or no arduino is connected, we rely on keyboard input.")]
    [SerializeField] ArduinoCommunicationModule arduino;
    [Tooltip("How many presets can you create? These will bind to (n) number inputs starting at 1 and the save button will be the (n+1) number key.")]
    [SerializeField] int numberOfPresetButtons;
    [SerializeField] TextMeshPro freq;
    enum RadioState { DEFAULT, AWAITING_SAVE_SELECTION }
    private RadioState state;
    private KeyCode savePresetKey;
    private const int maxRadioFreq = 100, minRadioFreq = 95, minVol = 0, maxVol = 38;
    private int freqRange, volRange;
    private float frequency = 100, volume = 15;
    public Radio() {
        freqRange = maxRadioFreq - minRadioFreq;
        volRange = (maxVol - minVol) + 1;
        state = RadioState.DEFAULT;
        savePresetKey = KeyCode.Alpha0 + numberOfPresetButtons;
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
        float freq = Mathf.Round((minRadioFreq + value * freqRange)*10)/10;
        SetFrequency(freq);
    }
    public void ButtonPress(int buttonIndex) { 
        //if(button)
    }

    // set radio to an exact frequency (i.e. 99.5f)
    private void SetFrequency(float freq) {
        // don't do anything if the frequency hasn't actually changed
        if (this.frequency == freq) {
            return;
        }
        if (this.state == RadioState.AWAITING_SAVE_SELECTION) { 
        
        }
        this.frequency = freq;
        RefreshFrequencyLCD();
    }
    private void CancelPresetSaving() {
        RefreshFrequencyLCD();
        state = RadioState.DEFAULT;
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
        if (!arduino) {
            return;
        }
        arduino.SendToLCD(frequency + "");
        freq.text = frequency + "";
    }
    // Update is called once per frame
    void Update()
    {

        for (KeyCode keyCode = KeyCode.Alpha0; keyCode <= savePresetKey; keyCode++)
        {
            if (!Input.GetKeyDown(keyCode))
            {
                continue;
            }
            if (keyCode == savePresetKey)
            {
                // save
            }
            else { 
                // select preset
            }
            break;
        }
    }
}