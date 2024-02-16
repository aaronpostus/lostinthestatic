using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDeer : MonoBehaviour
{
    public int numHits = 0;

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log(col.GetComponent<Rigidbody>().velocity.magnitude);
            if (col.GetComponent<Rigidbody>().velocity.magnitude >= 10)
            {
                numHits += 1;
            }
        }
    }

    void Update()
    {
        if (numHits >= 3)
        {
            Destroy(gameObject);
        }
    }
}