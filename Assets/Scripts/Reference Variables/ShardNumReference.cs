using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShardNumReference", menuName = "ScriptableObjects/ShardNumReference")]
public class ShardNumReference : ScriptableObject
{

    public event Action OnChange;
    
    public int Value { get { return value; } 
        set {
            this.value = value;
            OnChange?.Invoke();
        } 
    }

    private int value = 0;
}
