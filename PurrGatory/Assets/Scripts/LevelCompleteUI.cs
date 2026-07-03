using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI levelCompleteText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        double timeLastLevel = GameManager.Instance.GetTimeLastLevel();
        double timeRemaining = GameManager.Instance.GetTimeTillDawn();
        levelCompleteText.text = "Level Compelted\n"+"in \n"+ TimeSpan.FromSeconds(timeLastLevel).Minutes + ":" + TimeSpan.FromSeconds(timeLastLevel).Seconds + "\n" + "Kittens Saved\n " + GameManager.Instance.GetKittensSavedThisLevel() + "\n" + "Kittens Saved Overall\n " + GameManager.Instance.GetKittensSaved() + "\n" + "Time till Dawn\n" + TimeSpan.FromSeconds(timeRemaining).Minutes + ":" + TimeSpan.FromSeconds(timeRemaining).Seconds;

    }


    public void QuitButton()
    {
        Application.Quit();
    }

    public void NextLevelButton()
    {
        GameManager.Instance.LevelComplete();
    }
}
