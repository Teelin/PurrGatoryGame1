using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelCompleteText;

    private void OnEnable()
    {
        float timeLastLevel = GameManager.Instance.GetTimeLastLevel();
        float timeRemaining = GameManager.Instance.GetTimeTillDawn();
        levelCompleteText.text = "Level Compelted\n" + "in \n" + TimeSpan.FromSeconds(timeLastLevel).Minutes + ":" + TimeSpan.FromSeconds(timeLastLevel).Seconds + "\n" + "Kittens Saved\n " + GameManager.Instance.GetKittensSavedThisLevel() + "\n" + "Kittens Saved Overall\n " + GameManager.Instance.GetKittensSaved() + "\n" + "Time till Dawn\n" + TimeSpan.FromSeconds(timeRemaining).Minutes + ":" + TimeSpan.FromSeconds(timeRemaining).Seconds;
    }


    public void NextLevelButton()
    {
        GameManager.Instance.LevelComplete();
    }
    public void GoToMainMneu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitButton()
    {
        SceneManager.LoadScene("EndLevel");
    }

    public void ApophisButton()
    {
        Debug.Log("Apophis Button Pressed");
        SceneManager.LoadScene("Apophis");
    }

}
