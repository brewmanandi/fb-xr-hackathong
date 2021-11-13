public interface IGestureChecker {
    void InitGestureData(OVRHand leftHandData, OVRHand rightHandData);
    bool GetPinchGesture(OVRHand.Hand hand, OVRHand.HandFinger finger);
    bool GetPassthroughPinch(OVRHand.Hand hand);
    float GetCustomGestureDistance(OVRHand.Hand hand, CustomGestures gesture);
    bool GetCustomGesture(OVRHand.Hand hand, CustomGestures gesture);
    OVRCustomSkeleton LeftHandSkeleton { get; }
    OVRCustomSkeleton RightHandSkeleton { get; }
}