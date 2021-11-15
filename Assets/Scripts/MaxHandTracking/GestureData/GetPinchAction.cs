using System;
using TMPro;
using UnityEngine;

public class GetPinchAction : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI leftPinchDebugText;
    [SerializeField]
    private TextMeshProUGUI rightPinchDebugText;
    [SerializeField]
    private PinchAction leftPinchAction;
    [SerializeField]
    private PinchAction rightPinchAction;

    private void Update() {
        leftPinchDebugText.text = GetDebugTextPinch(OVRHand.Hand.HandLeft, leftPinchAction.Value);
        rightPinchDebugText.text = GetDebugTextPinch(OVRHand.Hand.HandRight, rightPinchAction.Value);
    }

    private string GetDebugTextPinch(OVRHand.Hand handedness, bool value) {
        return $"Pinch {Enum.GetName(typeof(OVRHand.Hand), handedness)}: {value}";
    }
}
