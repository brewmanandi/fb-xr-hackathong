using UnityEngine;

public class VoiceTest : MonoBehaviour {
    public void CheckVoiceResults(string[] results) {
        foreach (string result in results) {
            if (!result.Equals(string.Empty)) {
                Debug.Log("Received response: " + result);
            }
        }
    }
}
