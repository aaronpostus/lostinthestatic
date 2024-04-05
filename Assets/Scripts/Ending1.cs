using FMOD.Studio;
using FMODUnity;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using static SubtitleManager;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.Collections;
public class Ending1 : MonoBehaviour
{
    public float rotationSpeed = 30f; // Speed of rotation in degrees per second
    private TimelineInfo timelineInfo;
    private GCHandle timelineHandle;
    private FMOD.Studio.EVENT_CALLBACK beatCallback;
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.R)) {
            Restart();
        }
    }
    public void Restart()
    {
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

    void Start()
    {
        // Rotate the GameObject around its up axis (Y axis) continuously


        this.loopEventInstance = FMODUnity.RuntimeManager.CreateInstance(endingAudio);

        loopEventInstance.setVolume(1.0f);
        SubtitleManager.Instance.ClearSubtitles();
        loopEventInstance.start();

        SubtitleManager.Instance.AddCallback(loopEventInstance);
    }

    public EventReference endingAudio;
    private FMOD.Studio.EventInstance loopEventInstance;

}