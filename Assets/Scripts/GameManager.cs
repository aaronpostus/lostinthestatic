using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] StringReference puzzleCongrats;
    [SerializeField] ShardNumReference shards;
    [SerializeField] GameObject puzzleMiniMapPrefab;
    [SerializeField] List<Transform> puzzleLocationsMiniMap;
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
    private readonly List<GameObject> puzzleUIElements = new List<GameObject>();

    public PuzzleFlag PuzzleState = 0;

    public PlayerState TargetState { get; private set; }
    public PlayerState ActiveState { get; private set; }
    public CharacterMotor Player;
    public CarController Car;
    public CameraController Camera;
    public bool mainMenu = false;

    private void Awake()
    {
        instance = this;
        TransitionProgress = 0;
        ActiveState = PlayerState.OnFoot;
        if (mainMenu) {
            Debug.Log("test");
            ActiveState = PlayerState.InCar;
        }
        CarHandle.OnTryTransition += TryTransition;

        PlacePuzzleUIElements();

        if(puzzleCongrats!= null) puzzleCongrats.Value = "";
        if (shards != null) shards.Value = 0;
    }
    private void PlacePuzzleUIElements()
    {
        foreach (Transform loc in puzzleLocationsMiniMap) {
            Debug.Log("Placed UI element");
            puzzleUIElements.Add(Instantiate(puzzleMiniMapPrefab,loc));
        }
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

        // ew ew ew
        int index = 0;
        switch (puzzleCompleted) {
            case PuzzleFlag.Glass: index = 0; break;
            case PuzzleFlag.Deer: index = 1; break;
            case PuzzleFlag.Scale: index = 2; break;
            case PuzzleFlag.Corridor: index = 3; break;
            case PuzzleFlag.Maze: index = 4; break;
        }
        puzzleUIElements[index].transform.GetChild(0).gameObject.SetActive(false);
        puzzleUIElements[index].transform.GetChild(1).gameObject.SetActive(true);

        StartCoroutine(RemovePuzzleCongrats());
    }
    IEnumerator RemovePuzzleCongrats()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        puzzleCongrats.Value = "";
    }

    private void IncrementTransition() {
        TransitionProgress += Time.deltaTime / transitionTime;
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