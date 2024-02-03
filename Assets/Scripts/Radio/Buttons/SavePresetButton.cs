using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

public class SavePresetButton : IButton
{
    private Radio radio;
    private float savedRadioStation;
    private bool buttonPressed = false;
    public SavePresetButton(Radio radio)
    {
        this.radio = radio;
    }
    public void Press()
    {
        if (buttonPressed) {
            // button already pressed;
            return;
        }
        this.buttonPressed = true;
        this.radio.EnterSaveState();
    }

    public void Release()
    {
        if (!buttonPressed) {
            return;
        }
        buttonPressed = false;
    }
}
