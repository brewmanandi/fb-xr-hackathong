using UnityEngine;

public class GestureChecker : MonoBehaviour, IGestureChecker {

    [SerializeField]
    private OVRHand leftHand;
    [SerializeField]
    private OVRHand rightHand;
    [SerializeField]
    private GameObject customPinchBallPrefab;
    [SerializeField]
    private float pinchThreshold;

    private IGestureInterpreter gestureInterpreter;
    private GameObject leftCustomPinchball;
    private GameObject rightCustomPinchball;

    private Vector3 rightThumbTipPos;
    private Vector3 leftThumbTipPos;

    private Vector3 rightIndexTipPos;
    private Vector3 leftIndexTipPos;

    public OVRCustomSkeleton LeftHandSkeleton { get; private set; }
    public OVRCustomSkeleton RightHandSkeleton { get; private set; }

    private bool areBonesAvailable => LeftHandSkeleton.Bones.Count >= 20 && RightHandSkeleton.Bones.Count >= 20;

    private void Start() {
        if ((leftHand != null) && (rightHand != null)) {
            InitGestureData(leftHand, rightHand);
        }

        //leftCustomPinchball = GameObject.Instantiate(customPinchBallPrefab, Vector3.zero, Quaternion.identity);
        //rightCustomPinchball = GameObject.Instantiate(customPinchBallPrefab, Vector3.zero, Quaternion.identity);
    }

    private void Update() {
        CheckForBoneAvailability();
        //leftCustomPinchball.transform.position = CenterPosition(leftThumbTipPos, leftIndexTipPos);
        //rightCustomPinchball.transform.position = CenterPosition(rightThumbTipPos, rightIndexTipPos);
    }

    private void CheckForBoneAvailability() {
        if (areBonesAvailable) {
            leftThumbTipPos = LeftHandSkeleton.Bones[19].Transform.position;
            rightThumbTipPos = RightHandSkeleton.Bones[19].Transform.position;

            leftIndexTipPos = LeftHandSkeleton.Bones[20].Transform.position;
            rightIndexTipPos = RightHandSkeleton.Bones[20].Transform.position;
        }
    }

    private Vector3 CenterPosition(Vector3 firstPostion, Vector3 secondPosition) {
        return (firstPostion + secondPosition) * 0.5f;
    }

    public void InitGestureData(OVRHand leftHandData, OVRHand rightHandData) {
        leftHand = leftHandData;
        rightHand = rightHandData;

        LeftHandSkeleton = leftHandData.GetComponent<OVRCustomSkeleton>();
        RightHandSkeleton = rightHandData.GetComponent<OVRCustomSkeleton>();

        gestureInterpreter = GetComponent<IGestureInterpreter>();
    }

    public bool GetPinchGesture(OVRHand.Hand hand, OVRHand.HandFinger finger) {
        if ((leftHand == null) || (rightHand == null) ||
            leftHand.IsSystemGestureInProgress || rightHand.IsSystemGestureInProgress) {
            return false;
        }

        switch (hand) {
            case OVRHand.Hand.HandLeft:
                return leftHand.GetFingerIsPinching(finger);
            case OVRHand.Hand.HandRight:
                return rightHand.GetFingerIsPinching(finger);
            default:
                return false;
        }
    }

    public bool GetPassthroughPinch(OVRHand.Hand hand) {
        if ((leftHand == null) || (rightHand == null) ||
            leftHand.IsSystemGestureInProgress || rightHand.IsSystemGestureInProgress) {
            return false;
        }

        return CheckForPassthroughIndexFingerPinch(hand);
    }

    private bool CheckForPassthroughIndexFingerPinch(OVRHand.Hand hand) {
        switch (hand) {
            case OVRHand.Hand.HandLeft:
                return Vector3.Distance(leftIndexTipPos, leftThumbTipPos) < pinchThreshold;
            case OVRHand.Hand.HandRight:
                return Vector3.Distance(rightIndexTipPos, rightThumbTipPos) < pinchThreshold;
            default:
                return false;
        }
    }

    public float GetCustomGestureDistance(OVRHand.Hand hand, CustomGestures gesture) {
        if (gestureInterpreter != null) {
            return gestureInterpreter.CheckCustomGestureDistance(hand, RightHandSkeleton, gesture);
        } else {
            return 0f;
        }
    }

    public bool GetCustomGesture(OVRHand.Hand hand, CustomGestures gesture) {
        if (gestureInterpreter != null) {
            switch (hand) {
                case OVRHand.Hand.HandLeft:
                    return gestureInterpreter.CheckCustomGesture(hand, LeftHandSkeleton, gesture);
                case OVRHand.Hand.HandRight:
                    return gestureInterpreter.CheckCustomGesture(hand, RightHandSkeleton, gesture);
                default:
                    return false;
            }
        } else {
            return false;
        }
    }
}