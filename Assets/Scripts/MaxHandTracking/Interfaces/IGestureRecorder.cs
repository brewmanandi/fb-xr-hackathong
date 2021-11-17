using System.Collections.Generic;

public interface IGestureRecorder {
    void LoadCustomGesturesManually();
    List<GestureData> GetLeftHandGestures();
    List<GestureData> GetRightHandGestures();
    void ResetGestureRecordings();
    void ResetLeftHandGestureRecording();
    void ResetRightHandGestureRecording();
    void RecordGesture(OVRSkeleton handSkeleton, OVRHand.Hand handType, string gestureName);
    void ExportGesturesToFile();
}