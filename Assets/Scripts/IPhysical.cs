using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhysical
{
    public Vector3 velocity {
        get;
    }

    public Vector3 position {
        get;
    }
}
