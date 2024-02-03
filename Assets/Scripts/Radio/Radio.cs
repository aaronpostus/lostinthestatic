using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Radio : MonoBehaviour
{
    [Tooltip("Attach the module that feeds the radio input from an arduino.")]
    [SerializeField] ArduinoCommunicationModule arduino;
    [Tooltip("How many presets can you create? These will bind to (n) number inputs starting at 1 and the save button will be the (n+1) number key.")]
    [SerializeField] int numberOfPresetButtons;
    [Tooltip("Add all button text here for the preset channels")]
    [SerializeField ] List<TextMeshPro> buttonTexts;
    [SerializeField] TextMeshPro freq, vol;
    [Tooltip("Speed modifiers for keyboard controls for tuning dials")]
    [SerializeField] float volumeSpeed = 5f, radioSpeed = 1.5f;
    [Tooltip("How long should we allow the radio to stay in the save state before just returning to the normal state?")]
    [SerializeField] float saveStateMaxLength = 2f;
    enum RadioState { DEFAULT, AWAITING_SAVE_SELECTION, IDLING_AT_PRESET }
    private RadioState state;

    private List<IButton> buttons;

    private KeyCode savePresetKey;
    private const int maxRadioFreq = 100, minRadioFreq = 95, minVol = 0, maxVol = 38;

    // constants calculated at runtime
    private int freqRange, volRange;
    // raw floating point numbers that wont look how the user wants
    private float rawFrequency = 100, rawVolume = 15;
    // the actual numbers required for functionality and visual components
    private float prettyFrequency, prettyVolume;
    // timer for save state to expire
    private float remainingSaveStateTime;

    public Radio() {
        freqRange = maxRadioFreq - minRadioFreq;
        volRange = (maxVol - minVol) + 1;
        state = RadioState.DEFAULT;
        savePresetKey = KeyCode.Alpha1 + numberOfPresetButtons;

    }
    public void Awake()
    {
        buttons = new List<IButton>();
        for (int i = 0; i < numberOfPresetButtons; i++)
        {
            buttons.Add(new PresetButton(this, buttonTexts[i]));
        }
        buttons.Add(new SavePresetButton(this));
        RefreshFrequency();
        RefreshVolume();
    }
    public void JumpToPreset(float frequency) {
        this.state = RadioState.IDLING_AT_PRESET;
        this.prettyFrequency = frequency;
        RefreshFrequency();
    }
    public void EnterSaveState() {
        this.state = RadioState.AWAITING_SAVE_SELECTION;
        this.remainingSaveStateTime = this.saveStateMaxLength;
        if (Serial.CheckOpen())
        {
            Serial.WriteLn("PICK A PRESET");
        }
        this.freq.text = "CHOOSE A PRESET TO SAVE";
    }
    public float GetFrequency() { return prettyFrequency; }
    public float GetVolume() { return prettyVolume; }
    public KeyCode GetPresetKey() { return savePresetKey; }
    public void IncreaseVolume(float modifier) {
        modifier = rawVolume + (modifier * volumeSpeed);
        SetVolume(Mathf.Min(modifier, maxVol));
    }
    public void DecreaseVolume(float modifier)
    {
        modifier = rawVolume - (modifier * volumeSpeed);
        SetVolume(Mathf.Max(modifier, minVol));
    }
    public void IncreaseFrequency(float modifier)
    {
        modifier = rawFrequency + (radioSpeed * modifier);
        SetFrequency(Mathf.Min(modifier, maxRadioFreq));
    }
    public void DecreaseFrequency(float modifier)
    {
        modifier = rawFrequency - (radioSpeed * modifier);
        SetFrequency(Mathf.Max(modifier, minRadioFreq));
    }

    // tune radio volume based on a float value 0f -> 1f
    public void TuneVolume(float value)
    {
        // Ensure the input value is between 0 and 1
        value = Mathf.Max(0, Mathf.Min(1, value));
        // Map the input value to the integer range
        int result = minVol + (int)(value * volRange);
        SetVolume(result);
    }
    // tune radio station based on a float value 0f -> 1f
    public void TuneRadio(float value)
    {
        // Ensure the input value is between 0 and 1
        value = Mathf.Max(0, Mathf.Min(1, value));
        // Map the input value to the output range
        float freq = Mathf.Round((minRadioFreq + value * freqRange)*10)/10;
        SetFrequency(freq);
    }
    // isPress for press, !isPress for release
    public void ButtonInteraction(int buttonIndex, bool isPress) 
    {
        if (isPress)
        {
            this.buttons[buttonIndex].Press();
        }
        else
        {
            this.buttons[buttonIndex].Release();
        }
    }
    public bool TryingToSave() {
        return state == RadioState.AWAITING_SAVE_SELECTION;
    }

    // set radio to an exact frequency (i.e. 99.5f)
    private void SetFrequency(float freq) {
        // don't do anything if the frequency hasn't actually changed
        if (this.rawFrequency == freq) {
            return;
        }
        if (this.state == RadioState.AWAITING_SAVE_SELECTION) {
            this.state = RadioState.DEFAULT;
        }
        if (this.state == RadioState.IDLING_AT_PRESET) {
            this.state = RadioState.DEFAULT;
        }
        this.rawFrequency = freq;
        RefreshPrettyRadioFrequency();
        RefreshFrequency();
    }
    // set radio to an exact frequency (i.e. 99.5f)
    private void SetVolume(float vol)
    {
        // don't do anything if the volume hasn't actually changed
        if (this.rawVolume == vol) {
            return;
        }
        this.rawVolume = vol;
        RefreshPrettyVolume();
        RefreshVolume();
    }
    private void RefreshPrettyVolume() {
        this.prettyVolume = Mathf.Round(rawVolume);
    }
    private void RefreshPrettyRadioFrequency() { 
        this.prettyFrequency = Mathf.Round(rawFrequency * 10) / 10;
    }
    // updates the controller LCD and in-game elements
    private void RefreshFrequency() {
        string freq = this.prettyFrequency + "";
        if (Serial.CheckOpen())
        {
            Serial.WriteLn(freq);
        }
        this.freq.text = freq;
    }
    // updates the in-game elements
    private void RefreshVolume() {
        string volume = this.prettyVolume + "";
        this.vol.text = volume;
    }
    private void Update()
    {
    
        if (state == RadioState.AWAITING_SAVE_SELECTION) {
            if (this.remainingSaveStateTime <= 0f)
            {
                state = RadioState.DEFAULT;
                RefreshFrequency();
            }
            else {
                this.remainingSaveStateTime -= Time.deltaTime;
            }
        }
    }
}