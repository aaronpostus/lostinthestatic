using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] StringReference puzzleCongrats;
    [SerializeField] ShardNumReference shards;
    public EventReference puzzleSolvedNoise;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("[Game Manager]").AddComponent<GameManager>();
            return instance;
        }
    }
    private static GameManager instance;

    public static event Action<PlayerState> TransitionStarted;
    public static event Action<PlayerState> TransitionEnded;
    public float TransitionProgress { get; private set; }
    private readonly float transitionTime = 0.5f;
    public PuzzleFlag PuzzleState = 0;

    public PlayerState TargetState { get; private set; }
    public PlayerState ActiveState { get; private set; }
    public CharacterMotor Player;
    public CarController Car;
    public CameraController Camera;

    private void Awake()
    {
        instance = this;
        TransitionProgress = 0;
        ActiveState = PlayerState.OnFoot;
        Cursor.visible = false;
        CarHandle.OnTryTransition += TryTransition;


        if(puzzleCongrats!= null) puzzleCongrats.Value = "";
        if (shards != null) shards.Value = 0;
    }

    private void OnDestroy()
    {
        instance = null;
        CarHandle.OnTryTransition -= TryTransition;
    }

    private void TryTransition(PlayerState targetState)
    {
        if (TransitionProgress > 1 || targetState == ActiveState) return;
        TargetState = targetState;
        TransitionStarted?.Invoke(targetState); 
        UpdateTicker.Subscribe(IncrementTransition);
    }

    

    public void CompletePuzzle(PuzzleFlag puzzleCompleted)
    {
        PuzzleState = puzzleCompleted | PuzzleState;

        if (puzzleCongrats == null) return;
        puzzleCongrats.Value = "Glass shard collected.";
        FMODUnity.RuntimeManager.PlayOneShot(puzzleSolvedNoise, Player.position);
        shards.Value += 1;
        StartCoroutine(RemovePuzzleCongrats());
    }
    IEnumerator RemovePuzzleCongrats()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        puzzleCongrats.Value = "";
    }

    private void IncrementTransition() {
        TransitionProgress += Time.deltaTime / transitionTime;
        Debug.Log(TransitionProgress);
        if (TransitionProgress >= 1) {
            ActiveState = TargetState;
            TransitionProgress = 0;
            TransitionEnded?.Invoke(ActiveState);
            UpdateTicker.Unsubscribe(IncrementTransition);
        }
    }
}

public enum PlayerState
{
    InCar,
    OnFoot,
}

[Flags]
public enum PuzzleFlag
{
    Glass = 1,
    Deer = 2,
    Scale = 4,
    Corridor = 8,
    Maze = 16,
    Complete = 31
}