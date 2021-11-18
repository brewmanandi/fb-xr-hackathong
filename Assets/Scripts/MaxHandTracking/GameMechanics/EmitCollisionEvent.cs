using UnityEngine;
using UnityEngine.Events;

public class EmitCollisionEvent : MonoBehaviour {

    [SerializeField]
    private UnityEvent<string> cubeCollision;

    private void OnCollisionEnter(Collision collision) {
        cubeCollision.Invoke(collision.gameObject.name);
    }
}
