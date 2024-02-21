using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScalesPuzzleController : SerializedMonoBehaviour
{
    public ScalesFlag ItemsPlaced;
    [SerializeField] List<ScalesItem> Items;
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
        if (ItemsPlaced == ScalesFlag.Complete) GameManager.Instance.CompletePuzzle(PuzzleFlag.Scale);
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