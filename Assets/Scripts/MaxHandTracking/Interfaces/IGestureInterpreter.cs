using System.Collections.Generic;

public interface IGestureInterpreter {
    void SetHandGestureData(List<GestureData> leftGestureData, List<GestureData> rightGestureData);
    bool CheckCustomGesture(OVRHand.Hand hand, OVRSkeleton handSkeleton, CustomGestures customGesture);
    float CheckCustomGestureDistance(OVRHand.Hand hand, OVRSkeleton handSkeleton, CustomGestures customGesture);
}