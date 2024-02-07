using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    public static GameManager Instance {
        get {
            if (instance == null) instance = new GameObject("[Game Manager]").AddComponent<GameManager>();
            return instance;    
        }
    }
    private static GameManager instance;

    public static event Action<PlayerState> PlayerStateChanged;

    public PuzzleFlag PuzzleState = 0;

    public PlayerState TargetState;
    public PlayerState ActiveState {
        get {
            return activeState;
        }
        set {
            activeState = value;
            PlayerStateChanged?.Invoke(value);
        }
    }

    [SerializeField] private PlayerState activeState;
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
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void CompletePuzzle(PuzzleFlag puzzleCompleted) {
        PuzzleState = puzzleCompleted | PuzzleState;
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