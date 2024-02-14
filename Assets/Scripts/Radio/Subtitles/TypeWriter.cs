using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] StringReference referenceText, subtitleText;
    [SerializeField] PauseInfo pauseInfo;
    private int index;

    private void Start()
    {
        ReproduceText();
    }

    private void ReproduceText()
    {
        //if not readied all letters
        if (index < referenceText.Value.Length)
        {
            char letter = referenceText.Value[index];
            Write(letter);
            index += 1;
            StartCoroutine(PauseBetweenChars(letter));
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private string Write(char letter)
    {
        subtitleText.Value += letter;
        return subtitleText.Value;
    }

    private IEnumerator PauseBetweenChars(char letter)
    {
        switch (letter)
        {
            case '.':
                yield return new WaitForSeconds(pauseInfo.dotPause);
                ReproduceText();
                yield break;
            case ',':
                yield return new WaitForSeconds(pauseInfo.commaPause);
                ReproduceText();
                yield break;
            case ' ':
                yield return new WaitForSeconds(pauseInfo.spacePause);
                ReproduceText();
                yield break;
            default:
                yield return new WaitForSeconds(pauseInfo.normalPause);
                ReproduceText();
                yield break;
        }
    }
}

[Serializable]
public class PauseInfo
{
    public float dotPause;
    public float commaPause;
    public float spacePause;
    public float normalPause;
}