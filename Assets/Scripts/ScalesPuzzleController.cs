using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScalesPuzzleController : SerializedMonoBehaviour
{
    public ScalesFlag ItemsPlaced;
    [SerializeField] List<ScalesItem> Items;
    [SerializeField] Transform[] arms;
    [SerializeField] private Transform armPivot;
    [SerializeField] private float balanceTime = 0.5f, timer;
    public void Update()
    {
        for(int i=0; i<arms.Length; i++)
        {
            arms[i].rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        }   
    }

    public void OnEnable()
    {
        foreach (ScalesItem item in Items) item.OnPlaced += PlaceObject;
    }

    public void OnDisable()
    {
        foreach (ScalesItem item in Items) item.OnPlaced -= PlaceObject;
    }

    public void PlaceObject(ScalesItem item, bool placed) {
        ItemsPlaced = placed ? ItemsPlaced | item.ItemType : ItemsPlaced ^ item.ItemType;
        if (ItemsPlaced == ScalesFlag.Complete) {
            GameManager.Instance.CompletePuzzle(PuzzleFlag.Scale);
            foreach (ScalesItem allItem in Items) {
                Destroy(allItem);
            }
            UpdateTicker.Subscribe(IncPos);
        }
    }

    public void IncPos() {
        timer += Time.deltaTime;

        float prog = Mathf.Clamp01(timer / balanceTime);
        armPivot.localRotation = Quaternion.Euler(Mathf.Lerp(-10 , 10f, prog), 0,0);


        if(timer > balanceTime) {
            UpdateTicker.Unsubscribe(IncPos);
        }
    }
}

[Flags]
public enum ScalesFlag
{
    None = 0,
    Keys = 1,
    Wallet = 2,
    Bottle = 4,
    Cuffs = 8,
    Complete = 15
}