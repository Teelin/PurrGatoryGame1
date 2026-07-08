using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    float currentHealth;
    bool isPlayerNear = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        PlayerController.rattleAction += TakeDamage;
        Room.BossRoomEntered.AddListener(SetIsPlayerNear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetCurrentHealth() { return currentHealth; }

    void SetIsPlayerNear() { isPlayerNear = true; }
    void TakeDamage()
    {
        if (isPlayerNear)
        {
            currentHealth -= GameManager.Instance.GetPlayerAttackDamage();
            Debug.Log("Boss took damage! Current health: " + currentHealth);
            if (currentHealth <= 0)
            {
                LevelManager.Instance.isBossDefeated = true;
                Destroy(gameObject);
            }
        }
    }
}
