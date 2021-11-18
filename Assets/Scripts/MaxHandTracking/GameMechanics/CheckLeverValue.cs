using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLeverValue : MonoBehaviour {

    [SerializeField]
    private AudioSource leverQuestion;

    public void CheckCurrentValue(float value) {
        if (value <= -44) {
            leverQuestion.Play();
        }
    }
}
