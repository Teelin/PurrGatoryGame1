using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI kittensSavedText,timeLeft;
    [SerializeField] Slider timeSlider;
    float currentTimeTillDawn;
    GameObject player;
    [SerializeField] Image[] lives;
    [SerializeField] Sprite[] livesSprites;

    private void Start()
    {
        timeSlider.maxValue = GameManager.Instance.GetMaxTimeTillDawn();
        currentTimeTillDawn = GameManager.Instance.GetTimeTillDawn();
        player = GameObject.FindGameObjectWithTag("Player");

    }
    void Update()
    {
        var currentTimeInLevel = LevelManager.Instance.GetTimeToCompleteLevel();
        timeSlider.value = ( GameManager.Instance.GetMaxTimeTillDawn()- currentTimeTillDawn) +currentTimeInLevel;
        
        kittensSavedText.text = LevelManager.Instance.GetKittensSaved() + "/" + LevelManager.Instance.GetKittensThisLevel() ;
        //timeLeft.text = "Time Left: " + TimeSpan.FromSeconds(timeRemaining).Minutes + ":" + TimeSpan.FromSeconds(timeRemaining).Seconds;

        if(LevelManager.Instance.GetKittensSaved() < LevelManager.Instance.GetKittensNeedSaving())
        {
            kittensSavedText.color = Color.red;
        }
        else
        {
            kittensSavedText.color = Color.white;
        }

        switch (GameManager.Instance.GetLives())
        {
            case 9:
                UpdateHealthSprite(9);
                break;
            case 8:
                UpdateHealthSprite(8);
                lives[8].sprite = livesSprites[4];
                break;
            case 7:
                UpdateHealthSprite(7);
                lives[7].sprite = livesSprites[4];
                break;
            case 6:
                UpdateHealthSprite(6);
                lives[6].sprite = livesSprites[4];
                break;
            case 5:
                UpdateHealthSprite(5);
                lives[5].sprite = livesSprites[4];
                break;
            case 4:
                UpdateHealthSprite(4);
                lives[4].sprite = livesSprites[4];
                break;
            case 3:
                UpdateHealthSprite(3);
                lives[3].sprite = livesSprites[4];
                break;
            case 2:
                UpdateHealthSprite(2);
                lives[2].sprite = livesSprites[4];
                break;
            case 1:
                UpdateHealthSprite(1);
                lives[1].sprite = livesSprites[4];
                break;
        }

    }

    void UpdateHealthSprite(int currentLife)
    {
        float damagePerecent = player.GetComponent<BastHealth>().GetCurrentDamage()/ player.GetComponent<BastHealth>().GetMaxDamage();

        if (player.GetComponent<BastHealth>().GetCurrentDamage() < player.GetComponent<BastHealth>().GetMaxDamage() * 0.25f) 
        {
            lives[currentLife-1].sprite = livesSprites[0];
        }
        else if (player.GetComponent<BastHealth>().GetCurrentDamage() < player.GetComponent<BastHealth>().GetMaxDamage() * 0.5f)
        {
            lives[currentLife-1].sprite = livesSprites[1];
        }
        else if (player.GetComponent<BastHealth>().GetCurrentDamage() < player.GetComponent<BastHealth>().GetMaxDamage() * 0.75f)
        {
            lives[currentLife-1].sprite = livesSprites[2];
        }
        else
        {
            lives[currentLife-1].sprite = livesSprites[3];
        }
    }
}
