using UnityEngine;

public class ServiceContainer : MonoBehaviour {

    [SerializeField]
    private GestureChecker gestureChecker;

    private void Awake() {
        ServiceLocator.Register<IGestureChecker>(gestureChecker);
    }

    private void OnDestroy() {
        ServiceLocator.Reset();
    }
}
