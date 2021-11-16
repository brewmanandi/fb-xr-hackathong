using System;
using TMPro;
using UnityEngine;

public class GetCustomGestureAction : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI leftGestureDebugText;
    [SerializeField]
    private TextMeshProUGUI rightGestureDebugText;
    [SerializeField]
    private TextMeshProUGUI leftPinchDebugText;
    [SerializeField]
    private TextMeshProUGUI rightPinchDebugText;

    private IGestureChecker gestureChecker;

    private void Start() {
        gestureChecker = ServiceLocator.Resolve<IGestureChecker>();
    }

    //private void Update() {
    //    leftGestureDebugText.text = GetDebugTextCustomGestures(OVRHand.Hand.HandLeft);
    //    rightGestureDebugText.text = GetDebugTextCustomGestures(OVRHand.Hand.HandRight);

    //    //leftPinchDebugText.text = GetDebugTextPinch(OVRHand.Hand.HandLeft);
    //    //rightPinchDebugText.text = GetDebugTextPinch(OVRHand.Hand.HandRight);
    //}

    private string GetDebugTextCustomGestures(OVRHand.Hand handedness) {
        CustomGestures currentHandGesture = QueryForGestures(handedness);
        return $"Gesture {Enum.GetName(typeof(OVRHand.Hand), handedness)}: {Enum.GetName(typeof(CustomGestures), currentHandGesture)}";
    }

    private string GetDebugTextPinch(OVRHand.Hand handedness) {
        return $"Pinch {Enum.GetName(typeof(OVRHand.Hand), handedness)}: {gestureChecker.GetPassthroughPinch(handedness)}";
    }

    private CustomGestures QueryForGestures(OVRHand.Hand handedness) {
        foreach (CustomGestures gesture in Enum.GetValues(typeof(CustomGestures))) {
            bool customGestureFound = gestureChecker.GetCustomGesture(handedness, gesture);

            if (customGestureFound) {
                return gesture;
            }
        }
        return CustomGestures.None;
    }

    public CustomGestures QueryForLeftHandGestures() {
        return QueryForGestures(OVRHand.Hand.HandLeft);
    }

    public CustomGestures QueryForRightHandGestures() {
        return QueryForGestures(OVRHand.Hand.HandRight);
    }
}
