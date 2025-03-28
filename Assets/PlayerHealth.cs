using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthUI healthUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHealth(maxHealth);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Prevent negative health
        Debug.Log("Current Health: " + currentHealth);
        healthUI.SetHealth(currentHealth);
    }
}
