using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartTunnel : MonoBehaviour
{
    [SerializeField] private float triggerFrequency;
    [SerializeField] private Vector3 resetPosition;
    [SerializeField] private GameObject radio;
    [SerializeField] private GameObject door;
    private bool puzzleNotSolved = true;
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            checkFrequency();
            if(puzzleNotSolved)
            {
                col.gameObject.transform.position = resetPosition;
            }
        }
    }

    private void checkFrequency()
    {
        if (radio.GetComponent<Radio>().GetFrequency() == triggerFrequency)
        {
            puzzleNotSolved = false;
            door.GetComponent<Animator>().SetBool("puzzleSolved", true);
        }
    }
}
