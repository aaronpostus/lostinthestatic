using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHelper : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Delete());
    }
    IEnumerator Delete() {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
