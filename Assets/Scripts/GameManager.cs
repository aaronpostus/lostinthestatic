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
    public GameManager Instance {
        get {
            if (instance == null) instance = new GameObject("[Game Mmnager]").AddComponent<GameManager>();
        
            return Instance;
        }
    }




    private GameManager instance;
    public PlayerState ActivePlayerState;
    public Transform Player, Car;
    public Transform PlayerCameraTarget, CarCameraTarget;


    private void Awake()
    {
        
    }
}
