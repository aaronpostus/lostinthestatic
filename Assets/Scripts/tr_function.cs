using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tr_function : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<ReverseGravityAbility>().FlipGravity();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<ReverseGravityAbility>().FlipGravity();
        }
    }
}
