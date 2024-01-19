using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tr_sound : MonoBehaviour
{

    public AudioSource enter_sound;
    public AudioSource stay_sound;
    public AudioSource exit_sound;

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            enter_sound.Play();
            stay_sound.Play();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            exit_sound.Play();
            stay_sound.Stop();
        }
    }

}
