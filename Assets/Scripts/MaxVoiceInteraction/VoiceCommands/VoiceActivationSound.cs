using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceActivationSound : MonoBehaviour {

    [SerializeField]
    private AudioSource startVoiceSound;
    [SerializeField]
    private AudioSource stopVoiceSound;

    public void PlayStartVoiceSound() {
        startVoiceSound.Play();
    }

    public void PlayStopVoiceSound() {
        stopVoiceSound.Play();
    }
}
