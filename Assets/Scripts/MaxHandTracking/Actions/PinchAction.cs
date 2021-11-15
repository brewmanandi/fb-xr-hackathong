using UnityEngine;
using Zinnia.Action;
using Zinnia.Process;

public class PinchAction : BooleanAction, IProcessable {

    [SerializeField]
    private OVRHand.Hand handedness;

    private IGestureChecker gestureChecker;

    protected override void Start() {
        gestureChecker = ServiceLocator.Resolve<IGestureChecker>();
    }

    public void Process() {
        Receive(gestureChecker.GetPassthroughPinch(handedness));
    }
}
