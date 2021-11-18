using UnityEngine;

public class StartMetaverseEscape : MonoBehaviour {

    [SerializeField]
    private AudioSource introduction;

    private bool initCompleted;

    private void Update() {
        if (!initCompleted && OVRPlugin.GetHandTrackingEnabled()) {
            introduction.Play();
            initCompleted = true;
        }
    }
}
