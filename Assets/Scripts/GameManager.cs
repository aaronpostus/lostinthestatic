using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("[Game Manager]").AddComponent<GameManager>();
            return instance;
        }
    }
    private static GameManager instance;

    public static event Action<PlayerState> PlayerStateChanged;

    public PuzzleFlag PuzzleState = 0;

    public PlayerState TargetState;
    public PlayerState ActiveState
    {
        get
        {
            return activeState;
        }
        set
        {
            activeState = value;
            PlayerStateChanged?.Invoke(value);
        }
    }

    [SerializeField] private PlayerState activeState;
    [SerializeField] StringReference puzzleCongrats;
    public CharacterMotor Player;
    public CarController Car;
    public CameraController Camera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerStateChanged?.Invoke(activeState);
        puzzleCongrats.Value = "";
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void CompletePuzzle(PuzzleFlag puzzleCompleted)
    {
        PuzzleState = puzzleCompleted | PuzzleState;
        puzzleCongrats.Value = "Congrats on beating the " + puzzleCompleted + " puzzle.";
        StartCoroutine(RemovePuzzleCongrats());
    }
    IEnumerator RemovePuzzleCongrats()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        puzzleCongrats.Value = "";
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