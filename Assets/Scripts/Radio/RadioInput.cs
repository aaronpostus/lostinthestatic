using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class is used for handling keyboard input to control the radio. This could easily be refactored into a general user input class later on.
public class RadioInput : MonoBehaviour
{
    Dictionary<KeyCode, Action<float>> keyPressInputActions;
    Dictionary<KeyCode, Action<int, bool>> keyPressAndReleaseInputActions;
    [SerializeField] Radio radio;

    private void Awake()
    {
        keyPressInputActions = new Dictionary<KeyCode, Action<float>>
        {
            { KeyCode.RightArrow, radio.IncreaseVolume },
            { KeyCode.LeftArrow, radio.DecreaseVolume },
            { KeyCode.UpArrow, radio.IncreaseFrequency },
            { KeyCode.DownArrow, radio.DecreaseFrequency },
            { KeyCode.R, Restart }
        };
        // gahhh this is more hard coded than necessary but i couldnt get the commented out code to work difauhyujdfksajfdaj ill fix it later
        keyPressAndReleaseInputActions = new Dictionary<KeyCode, Action<int, bool>> 
        { 
            { KeyCode.Alpha1, radio.ButtonInteraction },
            { KeyCode.Alpha2, radio.ButtonInteraction },
            { KeyCode.Alpha3, radio.ButtonInteraction },
            { KeyCode.Alpha4, radio.ButtonInteraction },
            { KeyCode.Alpha5, radio.ButtonInteraction } 
        };
    }
    public void Restart(float throwaway) {
        StartCoroutine(nameof(Finish));
    }
    protected IEnumerator Finish()
    {
        FMODUnity.RuntimeManager.GetBus("Bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        var go = GameObject.Find("FMOD.UnityItegration.RuntimeManager");
        Destroy(go);

        // Skip a frame to allow full destruction
        yield return null;

        // Manually wipe isQuitting to prevent false positive errors from firing
        // This allows the RuntimeManager to perform a full initialization next time it's called
        var field = typeof(RuntimeManager).GetField("isQuitting",
            BindingFlags.Static |
            BindingFlags.NonPublic);

        if (field != null)
        {
            field.SetValue(null, false);
        }
        else
        {
            Debug.LogWarning("Could not find RuntimeManager.isQuitting");
        }
        SceneManager.LoadScene("Physics Testing");
    }
    private void Update()
    {
        foreach (KeyCode keyCode in keyPressInputActions.Keys) {
            if (Input.GetKey(keyCode)) { keyPressInputActions[keyCode].Invoke(Time.deltaTime); }
        }
        foreach (KeyCode keyCode in keyPressAndReleaseInputActions.Keys)
        {
            keyPressAndReleaseInputActions[keyCode].Invoke((int)(keyCode - 49), Input.GetKey(keyCode));
        }
    }
}

