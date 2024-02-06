using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Interact;

    // 0 means no successful pitches
    private static int currentIndex = 0;
    private static bool puzzleComplete = false;

    //tones indexed from 1 to 5
    [SerializeField] private int toneIndex;

    public void Interact()
    {
        //player has clicked on Glass
        Debug.Log("Play tone " + toneIndex);
        CheckSequence();
    }

    public bool CheckSequence()
    {
        if (puzzleComplete)
            return true;
        if (toneIndex - 1 == currentIndex)
        {
            currentIndex++;
            Debug.Log("Correct tone!");
            if (currentIndex >= 5)
            {
                puzzleComplete = true;
                Debug.Log("Happy Puzzle complete!");
            }
            return true;
        }
        Debug.Log("Sequence Failed");
        currentIndex = 0;
        return false;
    }
}