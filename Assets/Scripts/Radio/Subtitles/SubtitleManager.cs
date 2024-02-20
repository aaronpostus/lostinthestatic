using System.Runtime.InteropServices;
using System;
using UnityEngine;
using FMOD.Studio;

internal class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }
    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentMusicBar = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    private TimelineInfo timelineInfo;
    private GCHandle timelineHandle;
    private FMOD.Studio.EVENT_CALLBACK beatCallback;
    private static SubtitleManager instance;
    private GameObject typeWriter;
    [SerializeField] StringReference subtitleText, referenceText;
    [SerializeField] GameObject typeWriterPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        timelineInfo = new TimelineInfo();
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
    }
    private void Start()
    {
        subtitleText.Value = "";
        referenceText.Value = "";
    }
    private void OnDestroy()
    {
        instance = null;
    }
    public void AddCallback(EventInstance eventInstance)
    {
        eventInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        eventInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
    }
    public void RemoveCallback(EventInstance eventInstance)
    {
        eventInstance.setCallback(DummyCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.STARTED);
    }
    public void ClearSubtitles()
    {
        subtitleText.Value = "";
        referenceText.Value = "";
        Destroy(Instance.typeWriter);
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentMusicBar = parameter.bar;
                        break;
                    }
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;

                        if (Instance.typeWriter)
                        {
                            Instance.ClearSubtitles();
                            Debug.Log("A dialogue line was cut short. The speed needs to be increased or the text needs to be shorter.");
                        }

                        Instance.subtitleText.Value = "";
                        Instance.referenceText.Value = (string)parameter.name;
                        Instance.typeWriter = Instantiate(Instance.typeWriterPrefab);
                        break;
                    }
                case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                    {
                        // Now the event has been destroyed, unpin the timeline memory so it can be garbage collected
                        timelineHandle.Free();
                        break;
                    }
            }
        }
        return FMOD.RESULT.OK;
    }
    // Fmod is lacking a way to remove callbacks

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT DummyCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        return FMOD.RESULT.OK;
    }
}