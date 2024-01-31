using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

public class PresetButton : IButton
{
    private Radio radio;
    private float savedRadioStation = -1f;
    private bool buttonPressed = false;
    private TextMeshPro text;
    public PresetButton(Radio radio, TextMeshPro text) {
        this.radio = radio;
        this.text = text;
    }
    public void Press()
    {
        if (buttonPressed) {
            // button already held down
            return;
        }
        buttonPressed = true;
        if (radio.TryingToSave()) {
            // save current radio freq to this channel
            this.savedRadioStation = radio.GetFrequency();
            this.text.text = this.savedRadioStation + "";
        }
        if (savedRadioStation != -1f) {
            // this button is bound to a valid preset, change the frequency
            radio.JumpToPreset(this.savedRadioStation);
        }
    }

    public void Release()
    {
        if (!buttonPressed) {
            // button already released
            return;
        }
        buttonPressed = false;
        // rest of body intentionally blank
    }
}
