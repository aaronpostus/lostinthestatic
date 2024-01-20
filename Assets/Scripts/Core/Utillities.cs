using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public static class Utilities
{
    public static bool IsInLayer(LayerMask lm, int l)
    {
        return lm == (lm | (1 << l));
    }

    public static float Remap(float val, float in1, float in2, float out1, float out2)
    {
        if (val < in1) val = in1;
        if (val > in2) val = in2;

        float result = out1 + (val - in1) * (out2 - out1) / (in2 - in1);

        if (result < out1) result = out1;
        if (result > out2) result = out2;

        return result;
    }

    public static float Remap(float x, RangePair pair) {
        return Remap(x, pair.inRange.x, pair.inRange.y, pair.outRange.x, pair.outRange.y);
    }

    public static float easeInOutQuad(float x)
    {
        Mathf.Clamp01(x);
        return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }

    public static Vector3 GetRightVector(Vector3 vector, Vector3 up) {
        return Vector3.Cross(-vector, up).normalized;
    }

    public static float HorizontalDistance(Vector3 from, Vector3 to) {

        return new Vector3(to.x-from.x, 0, to.z-from.z).magnitude;
    }


    public static int Modulo(int a, int b) {
        return (a % b + b) % b;
        //-1-(-1/2)*2
    }
}

[System.Serializable]
public struct RangePair{
    public Vector2 inRange;
    public Vector2 outRange;
}
