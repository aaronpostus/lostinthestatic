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
            numHits += 1;
        }
    }

    void Update()
    {
        
    }
}
