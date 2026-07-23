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
    [SerializeField] Image rattleCooldown, sacraficeCooldown;

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

        if(GameManager.Instance.GetLives() == 9 && player.GetComponent<BastHealth>().GetCurrentDamage() == 0)
        {
            foreach (Image life in lives)
            {
                life.sprite = livesSprites[0];
            }
        }

        switch (GameManager.Instance.GetLives())
        {
            case 9:
                UpdateHealthSprite(9);
                break;
            case 8:
                UpdateHealthSprite(8);
                lives[8].sprite = livesSprites[8];
                break;
            case 7:
                UpdateHealthSprite(7);
                lives[8].sprite = livesSprites[8];
                lives[7].sprite = livesSprites[8];
                break;
            case 6:
                UpdateHealthSprite(6);
                lives[8].sprite = livesSprites[8];
                lives[7].sprite = livesSprites[8];
                lives[6].sprite = livesSprites[8];
                break;
            case 5:
                UpdateHealthSprite(5);
                lives[8].sprite = livesSprites[8];
                lives[7].sprite = livesSprites[8];
                lives[6].sprite = livesSprites[8];
                lives[5].sprite = livesSprites[8];
                break;
            case 4:
                UpdateHealthSprite(4);
                lives[8].sprite = livesSprites[8];
                lives[7].sprite = livesSprites[8];
                lives[6].sprite = livesSprites[8];
                lives[5].sprite = livesSprites[8];
                lives[4].sprite = livesSprites[8];
                break;
            case 3:
                UpdateHealthSprite(3);
                lives[8].sprite = livesSprites[8];
                lives[7].sprite = livesSprites[8];
                lives[6].sprite = livesSprites[8];
                lives[5].sprite = livesSprites[8];
                lives[4].sprite = livesSprites[8];
                lives[3].sprite = livesSprites[8];
                break;
            case 2:
                UpdateHealthSprite(2);
                lives[8].sprite = livesSprites[8];
                lives[7].sprite = livesSprites[8];
                lives[6].sprite = livesSprites[8];
                lives[5].sprite = livesSprites[8];
                lives[4].sprite = livesSprites[8];
                lives[3].sprite = livesSprites[8];
                lives[2].sprite = livesSprites[8];
                break;
            case 1:
                UpdateHealthSprite(1);
                lives[8].sprite = livesSprites[8];
                lives[7].sprite = livesSprites[8];
                lives[6].sprite = livesSprites[8];
                lives[5].sprite = livesSprites[8];
                lives[4].sprite = livesSprites[8];
                lives[3].sprite = livesSprites[8];
                lives[2].sprite = livesSprites[8];
                lives[1].sprite = livesSprites[8];
                break;
        }

        if(player.GetComponent<PlayerController>().GetRattleCooldown()<= 0 )
        {
            rattleCooldown.fillAmount = 1;
        }
        else
        {
            rattleCooldown.fillAmount = player.GetComponent<PlayerController>().GetRattleCooldown() / GameManager.Instance.GetRattleCooldown();
        }

        if (player.GetComponent<BastHealth>().GetSacrificeCooldownTimer() <= 0)
        {
            sacraficeCooldown.fillAmount = 1;
        }
        else
        {
            sacraficeCooldown.fillAmount = player.GetComponent<BastHealth>().GetSacrificeCooldownTimer() / player.GetComponent<BastHealth>().GetSacrificeCooldown();
            
        }

    }

    void UpdateHealthSprite(int currentLife)
    {
        switch (player.GetComponent<BastHealth>().GetCurrentDamage())
        {
            case 0:
                lives[currentLife-1].sprite = livesSprites[0];
                break;
            case 1:
                lives[currentLife-1].sprite = livesSprites[1];
                break;
            case 2:
                lives[currentLife-1].sprite = livesSprites[2];
                break;
            case 3:
                lives[currentLife-1].sprite = livesSprites[3];
                break;
            case 4:
                lives[currentLife - 1].sprite = livesSprites[4];
                break;
            case 5:
                lives[currentLife - 1].sprite = livesSprites[5];
                break;
            case 6:
                lives[currentLife - 1].sprite = livesSprites[6];
                break;
            case 7:
                lives[currentLife - 1].sprite = livesSprites[7];
                break;
            case 8:
                lives[currentLife - 1].sprite = livesSprites[8];
                break;
        }
    }
}
