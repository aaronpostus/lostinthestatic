using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Physics.gravity = Vector3.down * 9.8f;
        GameObject.FindWithTag("Player").GetComponent<CombinedCharacterController>().airJumps = 0;
        GameObject.FindWithTag("Player").GetComponent<ReverseGravityAbility>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
