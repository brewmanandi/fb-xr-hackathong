using UnityEngine;

public class VoiceActivator : MonoBehaviour {

    [SerializeField]
    private VoiceInteractionHandler voiceInteractionHandler;
    [SerializeField]
    private AudioSource startVoice;
    [SerializeField]
    private AudioSource stopVoice;

    private bool activationSoundToggle = true;

    public void VoiceActivation(bool isActivated) {
        Debug.Log("Button was pressed: " + isActivated);

        if (activationSoundToggle) {
            startVoice.Play();
        } else {
            stopVoice.Play();
        }
        activationSoundToggle = !activationSoundToggle;

        voiceInteractionHandler.ToggleActivation();
    }
}
