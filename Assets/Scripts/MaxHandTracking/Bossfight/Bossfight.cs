using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossfight : MonoBehaviour {

    [SerializeField]
    private GameObject bossFight;

    public void CheckCurrentValue(float value) {
        if (value <= -179) {
            bossFight.SetActive(true);
        }
    }
}
