using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassPuzzleController : MonoBehaviour
{

    [SerializeField] private List<Glass> sequence;
    [SerializeField] private int activeIndex;
    private HashSet<Glass> subscribedGlasses;

    private void Awake() {
        subscribedGlasses = new HashSet<Glass>();
        foreach (Glass glass in sequence)
        {
            subscribedGlasses.Add(glass);
        }
    }


    private void OnEnable() {
        foreach (Glass glass in subscribedGlasses)  glass.OnPlay += CheckSequence; 
    }

    

    private void OnDisable() {
        foreach (Glass glass in subscribedGlasses) glass.OnPlay -= CheckSequence;
    }


    private void CheckSequence(Glass glass) {
        if (sequence[activeIndex].Equals(glass)){
            activeIndex++;
            if (activeIndex >= sequence.Count) {
                GameManager.Instance.CompletePuzzle(PuzzleFlag.Glass);
                enabled = false;
            } 
            
        }
        else {
            activeIndex = 0;
        }
    }
}
