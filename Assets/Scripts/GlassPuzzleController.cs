using FMOD.Studio;
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
        foreach (Glass glass in GetComponentsInChildren<Glass>())
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

    private void RemoveGlasses() {
        // nasty bad change this later
        foreach (GameObject glass in GameObject.FindGameObjectsWithTag("Cup")) {
            Destroy(glass);
        }
    }
    private void CheckSequence(Glass glass) {
        if (sequence[activeIndex].Equals(glass)){
            activeIndex++;
            if (activeIndex >= sequence.Count) {
                GameManager.Instance.CompletePuzzle(PuzzleFlag.Glass);
                // Add a particle effect here for GOLD
                RemoveGlasses();
                enabled = false;
            } 
            
        }
        else {
            activeIndex = 0;
        }
    }
}
