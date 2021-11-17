using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Data.Type.Transformation.Conversion;

public class WristWatch : MonoBehaviour {

    [SerializeField]
    private FloatToBoolean float2Bool;

    public void CheckCurrentValue(float value) {
        Debug.Log("Current float value: " + value);
    }

    public void CheckCurrentValue(bool value) {
        Debug.Log("Current bool value: " + value);
    }
}
