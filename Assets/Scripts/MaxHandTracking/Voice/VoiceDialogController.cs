using UnityEngine;
using UnityEngine.UI;

public class VoiceDialogController:MonoBehaviour
{
    //Public Variables:
    public Button activateButton;
    public VoiceInteractionHandler voiceInteractionHandler;
    
    //Startup:
    private void OnEnable()
    {
        //hooks:
        activateButton.onClick.AddListener(HandleActivate);
    }
    
    //Shutdown:
    private void OnDisable()
    {
        //hooks:
        activateButton.onClick.RemoveListener(HandleActivate);
    }
    
    //Event Handlers:
    private void HandleActivate()
    {
        voiceInteractionHandler.ToggleActivation();
    }
}
