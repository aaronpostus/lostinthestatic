using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartTunnel : MonoBehaviour
{
    [SerializeField] private float triggerFrequency;
    [SerializeField] private Transform resetPosition;
    [SerializeField] private GameObject radio;
    [SerializeField] private Transform door;
    private bool puzzleNotSolved = true;
    private Vector3 positionOffset;

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            checkFrequency();
            if(puzzleNotSolved)
            {
                positionOffset = col.gameObject.transform.position - transform.position;
                col.gameObject.GetComponent<Rigidbody>().position = resetPosition.position+positionOffset;
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
