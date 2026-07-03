using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class HUDControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI kittensSavedText,timeLeft;

    void Update()
    {
        var currentTimeInLevel = LevelManager.Instance.GetTimeToCompleteLevel();
        var timeRemaining = 300 - currentTimeInLevel;
        kittensSavedText.text = "Kittens Saved: " + LevelManager.Instance.GetKittensSaved() + "/" + LevelManager.Instance.GetKittensThisLevel();
        timeLeft.text = "Time Left: " + TimeSpan.FromSeconds(timeRemaining).Minutes + ":" + TimeSpan.FromSeconds(timeRemaining).Seconds;

    }
}
