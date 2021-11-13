using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RockPaperScissors : MonoBehaviour {
    
    // player = attack
    // 0 = player is draw
    // 1 = player is winner
    // 2 = player is loser
    private int[,] rpsMatrix = new int[3, 3] {
// defend>r  p  s    // attack v
        { 0, 2, 1 }, // rock (r)
        { 1, 0, 2 }, // paper (p)
        { 2, 1, 0 }  // scissors (s)
    };

    [SerializeField]
    private List<GameObject> rpsCubes;
    [SerializeField]
    private TextMeshProUGUI leftRpsDebugText;
    [SerializeField]
    private TextMeshProUGUI rightRpsDebugText;
    [SerializeField]
    private GetCustomGestureAction getCustomGestureAction;

    private int currentCubeIndex;
    private int lastCubeIndex;

    private void Start() {
        foreach (GameObject cube in rpsCubes) {
            cube.SetActive(false);
        }
        lastCubeIndex = -1;

        InvokeRepeating("SetRandomCubeActive", 0, 3);
    }

    private void Update() {
        CustomGestures currentLeftHandGesture = getCustomGestureAction.QueryForLeftHandGestures();
        CustomGestures currentRightHandGesture = getCustomGestureAction.QueryForRightHandGestures();

        leftRpsDebugText.text = GetDebugTextRps((int)currentLeftHandGesture, currentCubeIndex);
        rightRpsDebugText.text = GetDebugTextRps((int)currentRightHandGesture, currentCubeIndex);
    }

    private string GetDebugTextRps(int attacker, int defender) {
        int result = rpsMatrix[attacker, defender];
        Debug.Log("Result: " + result);
        switch (result) {
            case 0:
                return "DRAW";
            case 1:
                return "YOU WIN";
            case 2:
                return "YOU LOSE";
            default:
                return "NO RESULT";
        }
    }

    private void SetRandomCubeActive() {
        if (lastCubeIndex >= 0) {
            rpsCubes[lastCubeIndex].SetActive(false);
        }

        currentCubeIndex = Random.Range(0, 3);
        rpsCubes[currentCubeIndex].SetActive(true);
        lastCubeIndex = currentCubeIndex;
    }
}
