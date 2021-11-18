using AssetConfigurator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
    private GameObject demonDog;

    [SerializeField]
    private AudioSource ambientAudio;
    [SerializeField]
    private AudioSource endbossAudio;
    [SerializeField]
    private AudioSource demonDogDefeated;
    [SerializeField]
    private AudioSource demonDogAttack;
    [SerializeField]
    private AudioSource demonDogHit;
    [SerializeField]
    private AudioSource demonDogHowl;
    [SerializeField]
    private AudioSource roundGoGoGo;

    [SerializeField]
    private List<GameObject> rpsCubes;
    [SerializeField]
    private TextMeshProUGUI leftRpsDebugText;
    [SerializeField]
    private TextMeshProUGUI rightRpsDebugText;
    [SerializeField]
    private TextMeshProUGUI roundStateDebugText;
    [SerializeField]
    private TextMeshProUGUI scoreDebugText;
    [SerializeField]
    private GetCustomGestureAction getCustomGestureAction;

    [SerializeField]
    private UnityEvent finishedBoss;

    private AssetConfigurationData target;

    private int currentCubeIndex;
    private int lastCubeIndex;
    private bool checkForResult;
    private bool gameOver;

    [SerializeField]
    private int score;

    private void Start() {
        foreach (GameObject cube in rpsCubes) {
            cube.SetActive(false);
        }
        score = 0;
        lastCubeIndex = -1;
        roundStateDebugText.text = "WAIT FOR NEXT\nROUND TO START";
        InvokeRepeating("SetRandomCubeActive", 3, 3);

        target = demonDog.GetComponent<AssetConfigurationData>();
        ambientAudio.Stop();
        endbossAudio.Play();

        // animation enter the scene
        target.TargetAnimator.Play(target.AnimationOptions[6].Animation.name);
        demonDogHowl.Play();
    }

    private void Update() {
        if (!gameOver) {
            CustomGestures currentLeftHandGesture = getCustomGestureAction.QueryForLeftHandGestures();
            CustomGestures currentRightHandGesture = getCustomGestureAction.QueryForRightHandGestures();

            if (checkForResult) {
                leftRpsDebugText.text = GetDebugTextRps((int)currentLeftHandGesture, currentCubeIndex);
            }
            if (checkForResult) {
                rightRpsDebugText.text = GetDebugTextRps((int)currentRightHandGesture, currentCubeIndex);
            }

            scoreDebugText.text = "Your score: " + score;

            CheckIfBossIsDefeated();
        }
    }

    private string GetDebugTextRps(int attacker, int defender) {
        string resultText = "NO RESULT";
        roundStateDebugText.text = "GOGOGOGOGO!!!";

        if (attacker >= 0) {
            int result = rpsMatrix[attacker, defender];

            switch (result) {
                case 0:
                    checkForResult = false;
                    resultText = Enum.GetName(typeof(CustomGestures), attacker) + " DRAW";
                    roundStateDebugText.text = "WAIT FOR NEXT\nROUND TO START";
                    break;
                case 1:
                    checkForResult = false;
                    resultText = Enum.GetName(typeof(CustomGestures), attacker) + " YOU WIN";
                    roundStateDebugText.text = "WAIT FOR NEXT\nROUND TO START";
                    score++;

                    // animation got hit
                    target.TargetAnimator.Play(target.AnimationOptions[6].Animation.name);
                    demonDogHit.Play();

                    break;
                case 2:
                    checkForResult = false;
                    resultText = Enum.GetName(typeof(CustomGestures), attacker) + " YOU LOSE";
                    roundStateDebugText.text = "WAIT FOR NEXT\nROUND TO START";
                    score--;

                    // animation attack
                    target.TargetAnimator.Play(target.AnimationOptions[2].Animation.name);
                    demonDogAttack.Play();
                    break;
            }
        }
        return resultText;
    }

    private void CheckIfBossIsDefeated() {
        if (score >= 3) {
            CancelInvoke();
            roundStateDebugText.text = "YOU WON!\nEXIT NOW!";
            // animation die
            target.TargetAnimator.Play(target.AnimationOptions[4].Animation.name);
            // unlock door

            gameOver = true;
            endbossAudio.Stop();
            demonDogDefeated.Play();
            finishedBoss.Invoke();
        }
    }

    private void SetRandomCubeActive() {
        if (lastCubeIndex >= 0) {
            rpsCubes[lastCubeIndex].SetActive(false);
        }

        currentCubeIndex = UnityEngine.Random.Range(0, 3);
        rpsCubes[currentCubeIndex].SetActive(true);
        lastCubeIndex = currentCubeIndex;
        checkForResult = true;
        roundGoGoGo.Play();
    }
}
