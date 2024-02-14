using System;
using UnityEngine;

public class PuzzleWinTrigger : MonoBehaviour
{
    [SerializeField] PuzzleFlag flagType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.CompletePuzzle(flagType);
            Destroy(this.gameObject);
        }
    }
}
