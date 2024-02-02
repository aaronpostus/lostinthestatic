using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    InCar,
    OnFoot,
    Transition
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {
        get {
            if (instance == null) instance = new GameObject("[Game Mmnager]").AddComponent<GameManager>();
            return Instance;
        }
    }
    private static GameManager instance;

    public PlayerState ActivePlayerState;

    public CharacterMotor Player;
    public CarController Car;

    public Transform CameraHolder;
    private Transform cameraTarget;

    public void TransitionPlayer(bool isEnter) {
        if (ActivePlayerState == PlayerState.Transition) return;
        if ((isEnter && ActivePlayerState == PlayerState.InCar) && (!isEnter && ActivePlayerState == PlayerState.OnFoot)) return;
        cameraTarget = isEnter ? Car.GetTarget() : Player.GetTarget();

    }
}
