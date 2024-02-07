using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearParentOnAwake : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = null;
    }
}
