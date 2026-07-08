using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI kittensSavedText,timeLeft, livesText;
    [SerializeField] Slider timeSlider;
    float currentTimeTillDawn;
    GameObject player;

    private void Start()
    {
        timeSlider.maxValue = 2400;
        currentTimeTillDawn = GameManager.Instance.GetTimeTillDawn();
        player = GameObject.FindGameObjectWithTag("Player");

    }
    void Update()
    {
        var currentTimeInLevel = LevelManager.Instance.GetTimeToCompleteLevel();
        timeSlider.value = (currentTimeTillDawn-2400f)+currentTimeInLevel;

        kittensSavedText.text = "Kittens Saved: " + LevelManager.Instance.GetKittensSaved() + "/" + LevelManager.Instance.GetKittensThisLevel() + "\n" + "Kittens needed: " + LevelManager.Instance.GetKittensNeedSaving();
        //timeLeft.text = "Time Left: " + TimeSpan.FromSeconds(timeRemaining).Minutes + ":" + TimeSpan.FromSeconds(timeRemaining).Seconds;

        livesText.text = "Lives: " + GameManager.Instance.GetLives() + "\n" + "Damage: " + player.GetComponent<BastHealth>().GetCurrentDamage() + "/" + player.GetComponent<BastHealth>().GetMaxDamage();

    }
}
