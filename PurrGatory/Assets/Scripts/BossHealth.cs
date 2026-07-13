using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    float currentHealth;
    bool isPlayerNear = false;
    GameObject player;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject bossGO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 20 * (GameManager.Instance.GetCurrentLevel()+1);

        currentHealth = maxHealth;

        PlayerController.rattleAction += TakeDamage;
        player = GameObject.FindGameObjectWithTag("Player");

    }
    private void OnDisable()
    {
        PlayerController.rattleAction -= TakeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerNear = Vector2.Distance(transform.position, player.transform.position) < player.GetComponent<PlayerController>().GetRattleRange();

        healthBar.fillAmount = currentHealth / maxHealth;
    }
    public float GetCurrentHealth() { return currentHealth; }

    public float GetMaxHealth() { return maxHealth; }
    void TakeDamage()
    {
        if (isPlayerNear)
        {
            currentHealth -= player.GetComponent<PlayerController>().GetRattleDamege();
            Debug.Log("Boss took damage! Current health: " + currentHealth);
            if (currentHealth <= 0)
            {
                LevelManager.Instance.isBossDefeated = true;
                Destroy(bossGO);
            }
        }
    }
}
