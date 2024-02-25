using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Printing;
using UnityEngine.Windows;

public class InputRecorder : MonoBehaviour, IInputProvider
{
    [SerializeField] private bool isRecording = false;
    [SerializeField] private string recordingFilePath;
    [SerializeField] private bool isPlayingBack = false;
    [SerializeField] private string playbackFilePath;
    [SerializeField] private GameObject car;

    private List<InputState> recordedInput = new List<InputState>();
    private List<InputState> playbackInput = new List<InputState>();
    private int currentInputIndex = 0;
    public PlayerInput input { get; private set; }
    public ButtonAction OnJump { get { return jumpAction; } }
    public ButtonAction OnSprint { get { return sprintAction; } }

    public ButtonAction OnInteract { get { return interactAction; } }

    public ButtonAction OnExit { get { return exitAction; } }

    private ButtonAction jumpAction, sprintAction, interactAction, exitAction;
    public void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();
        jumpAction = new ButtonAction(input.Game.Jump);
        sprintAction = new ButtonAction(input.Game.Sprint);
        interactAction = new ButtonAction(input.Game.Interact);
        exitAction = new ButtonAction(input.Game.Exit);

        car.GetComponent<Rigidbody>().position = transform.position;

        if (isPlayingBack) {
            StartPlayback(playbackFilePath);
        }
    }

    // Reference to the PlayerInputSO
    public PlayerInputSO playerInputSO;

    void FixedUpdate()
    {
        if (isRecording)
        {
            RecordInput();
        }
        if (isPlayingBack)
        {
            PlaybackInput();
        }
    }

    void RecordInput()
    {
        InputState inputState = playerInputSO.GetState();
        recordedInput.Add(inputState);
    }

    void SaveInputToFile(List<InputState> inputList, string filePath)
    {
        using StreamWriter writer = new StreamWriter(filePath);
        foreach (InputState input in inputList)
        {
            writer.WriteLine(input.ToString());
            Debug.Log(input.ToString());
        }
        writer.Close();
    }

    void LoadInputFromFile(List<InputState> inputList, string filePath)
    {
        inputList.Clear();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                if (values.Length == 4)
                {
                    float moveX = float.Parse(values[0]);
                    float moveY = float.Parse(values[1]);
                    float lookX = float.Parse(values[2]);
                    float lookY = float.Parse(values[3]);
                    InputState inputState = new InputState(new Vector2(moveX, moveY), new Vector2(lookX, lookY));
                    inputList.Add(inputState);
                }
            }
        }
    }

    void PlaybackInput()
    {
        if (currentInputIndex < playbackInput.Count)
        {
            InputState currentInput = playbackInput[currentInputIndex];
            currentInputIndex++;
        }
        else
        {
            currentInputIndex = 0;
            car.GetComponent<Rigidbody>().position = transform.position;
            car.GetComponent<Rigidbody>().rotation = Quaternion.identity;
            car.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("Finished");
        }
    }

    void OnDestroy() {
        if (isRecording) StopRecording();
    }
    public void StopRecording()
    {
        isRecording = false;
        SaveInputToFile(recordedInput, recordingFilePath);
    }

    public void StartPlayback(string filePath)
    {
        isPlayingBack = true;
        playbackFilePath = filePath;
        LoadInputFromFile(playbackInput, playbackFilePath);
    }

    public InputState GetState()
    {
        if (isRecording)
        {
            // Return current input during recording mode
            return playerInputSO.GetState();
        }
        else if (isPlayingBack && currentInputIndex < playbackInput.Count)
        {
            // Return recorded input during playback mode
            return playbackInput[currentInputIndex];
        }
        else
        {
            // Return default input state if not recording or playback is finished
            return new InputState(Vector2.zero, Vector2.zero);
        }
    }
}