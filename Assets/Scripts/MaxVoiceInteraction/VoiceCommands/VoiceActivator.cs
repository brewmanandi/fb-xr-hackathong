using UnityEngine;

public class VoiceActivator : MonoBehaviour {

    [SerializeField]
    private VoiceInteractionHandler voiceInteractionHandler;

    public void VoiceActivation(bool isActivated) {
        Debug.Log("Button was pressed: " + isActivated);
        voiceInteractionHandler.ToggleActivation();
    }
}
