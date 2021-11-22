using System;
using UnityEngine;

public class StartMetaverseEscape : MonoBehaviour {

    [SerializeField]
    private AudioSource introduction;
    /// <summary>
    /// subscribe to be notified when the VR environment has been placed and enabled
    /// </summary>
    public static event Action OnEnvironmentSet;
    private bool initCompleted;

    private void OnEnable()
    {
        OnEnvironmentSet?.Invoke();
    }

    private void Update() {
        if (!initCompleted && OVRPlugin.GetHandTrackingEnabled()) {
            introduction.Play();
            initCompleted = true;
        }
    }
}
