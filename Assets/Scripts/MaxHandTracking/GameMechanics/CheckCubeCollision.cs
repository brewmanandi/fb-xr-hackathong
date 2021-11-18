using UnityEngine;
using Zinnia.Tracking.Collision;

public class CheckCubeCollision : MonoBehaviour {

    [SerializeField]
    private AudioSource cubeQuestion;

    public void OnCubeCollisionStart(string collsionObjectName) {
        if (collsionObjectName.Contains("OculusWoodBlockPf")) {
            cubeQuestion.Play();
        }
    }
}
