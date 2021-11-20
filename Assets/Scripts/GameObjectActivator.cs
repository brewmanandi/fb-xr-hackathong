using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActivator : MonoBehaviour {
    [SerializeField] GameObject _gameObjectToActivate;
    [SerializeField] ActivationMethod _activationMethod = ActivationMethod.facingHMD;    

    private Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    private bool IsFacingCamera() {
        float dot = Vector3.Dot(_gameObjectToActivate.transform.up, _camera.transform.forward);
        return dot < 0;
    }


    private void Update() {

        switch(_activationMethod) {
            case ActivationMethod.facingHMD:
                _gameObjectToActivate.SetActive(IsFacingCamera());
                break;
            default:
                break;
        }
    }

}


public enum ActivationMethod {
none,
facingHMD,
byEvent,}
