using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour {

    [SerializeField]
    private List<GameObject> objectsToDisableOnStartup;

    private void Awake() {
        foreach (GameObject go in objectsToDisableOnStartup) {
            go.SetActive(false);
        }
    }
}
