using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI levelCompleteText,kittensToSpendText;
    
    public enum BuyType
    {
        RattleRange,
        RattleCooldown,
        RattleDamnage,
        SightRange,
        Speed,
        Life
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        double timeRemaining = GameManager.Instance.GetTimeTillDawn();
        double timeCompleted = 2400f - timeRemaining;
        levelCompleteText.text = "This Evenings Run was Compelted in \n"+ TimeSpan.FromSeconds(timeCompleted).Minutes + ":" + TimeSpan.FromSeconds(timeCompleted).Seconds + "\n" + "Kittens Saved Overall\n " + GameManager.Instance.GetKittensSaved() + "\n" + "Spend your souls and prepare for the next Evening";
        kittensToSpendText.text = GameManager.Instance.GetKittensAvailableToSpend().ToString();
    }


    public void QuitButton()
    {
        Application.Quit();
    }

    public void RestartRunButton()
    {
        GameManager.Instance.RoundOver();
    }

    public void BuyButtonPressed(string buyType)
    {
        switch (buyType)
        {
            case "RattleRange":
                if (GameManager.Instance.SpendKittens(5))
                {
                    // Successfully spent kittens, update UI or perform other actions
                    kittensToSpendText.text = GameManager.Instance.GetKittensAvailableToSpend().ToString();
                    GameManager.Instance.UpdateRattleRange(GameManager.Instance.GetRattleRange()*1.05f);
                }
                else
                {
                    // Not enough kittens available, show a warning or feedback
                    Debug.LogWarning("Not enough kittens available to spend.");
                }
                break;
            case "RattleCooldown":
                if (GameManager.Instance.SpendKittens(5))
                {
                    // Successfully spent kittens, update UI or perform other actions
                    kittensToSpendText.text = GameManager.Instance.GetKittensAvailableToSpend().ToString();
                    GameManager.Instance.UpdateRattleCooldown(GameManager.Instance.GetRattleCooldown() * 0.95f);
                }
                else
                {
                    // Not enough kittens available, show a warning or feedback
                    Debug.LogWarning("Not enough kittens available to spend.");
                }
                break;
            case "RattleDamage":
                if (GameManager.Instance.SpendKittens(5))
                {
                    // Successfully spent kittens, update UI or perform other actions
                    kittensToSpendText.text = GameManager.Instance.GetKittensAvailableToSpend().ToString();
                    GameManager.Instance.UpdateDamage(GameManager.Instance.GetDamage() * 1.05f);
                }
                else
                {
                    // Not enough kittens available, show a warning or feedback
                    Debug.LogWarning("Not enough kittens available to spend.");
                }
                break;
            case "SightRange":
                if (GameManager.Instance.SpendKittens(3))
                {
                    // Successfully spent kittens, update UI or perform other actions
                    kittensToSpendText.text = GameManager.Instance.GetKittensAvailableToSpend().ToString();
                    GameManager.Instance.UpdateSightRange(GameManager.Instance.GetSightRange() * 1.05f);
                }
                else
                {
                    // Not enough kittens available, show a warning or feedback
                    Debug.LogWarning("Not enough kittens available to spend.");
                }
                break;
            case "Speed":
                if (GameManager.Instance.SpendKittens(4))
                {
                    // Successfully spent kittens, update UI or perform other actions
                    kittensToSpendText.text = GameManager.Instance.GetKittensAvailableToSpend().ToString();
                    GameManager.Instance.UpdateSpeed(GameManager.Instance.GetSpeed() * 1.05f);
                }
                else
                {
                    // Not enough kittens available, show a warning or feedback
                    Debug.LogWarning("Not enough kittens available to spend.");
                }
                break;
            case "Life":
                if (GameManager.Instance.GetLives() < 9)
                {
                    if (GameManager.Instance.SpendKittens(2))
                    {
                        // Successfully spent kittens, update UI or perform other actions
                        kittensToSpendText.text = GameManager.Instance.GetKittensAvailableToSpend().ToString();
                        GameManager.Instance.AddLife();

                    }
                    else
                    {
                        // Not enough kittens available, show a warning or feedback
                        Debug.LogWarning("Not enough kittens available to spend.");
                    }
                }
                break;
        }       

    }
    
}
