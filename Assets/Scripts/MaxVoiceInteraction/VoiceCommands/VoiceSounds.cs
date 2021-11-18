using System.Collections.Generic;
using UnityEngine;

public class VoiceSounds : MonoBehaviour {

    [SerializeField]
    private List<VoiceElement> navigationSounds;
    [SerializeField]
    private List<VoiceElement> answerSounds;

    public void CheckNavigationVoiceResults(string[] results) {
        string result = FoundVoiceResult(results);
        if (!result.Equals(string.Empty)) {
            PlayNavigationSoundByName(result);
        }
    }

    public void CheckAnswerVoiceResults(string[] results) {
        string result = FoundVoiceResult(results);
        if (!result.Equals(string.Empty)) {
            PlayAnswerFromAnswers(result);
        } else {
            // wrong answer
            answerSounds[0].ElementSound.Play();
        }
    }

    private void PlayNavigationSoundByName(string result) {
        foreach (VoiceElement item in navigationSounds) {
            if (item.IsActive && item.Name.ToLower().Contains(result.ToLower())) {
                item.ElementSound.Play();
                if (result.Equals("continue")) {
                    item.IsActive = false;
                }
                break;
            }
        }
    }

    private void PlayAnswerFromAnswers(string result) {
        foreach (VoiceElement item in answerSounds) {
            if (item.IsActive && item.Name.ToLower().Contains(result.ToLower())) {
                item.ElementSound.Play();
                item.IsActive = false;
                break;
            }
        }
    }

    private string FoundVoiceResult(string[] results) {
        foreach (string result in results) {
            if (!result.Equals(string.Empty)) {
                return result;
            }
        }
        return string.Empty;
    }
}
