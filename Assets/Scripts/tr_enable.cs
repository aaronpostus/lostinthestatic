using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tr_enable : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<ReverseGravityAbility>().enabled = true;
            Destroy(gameObject);
        }
    }
}
