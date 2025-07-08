using UnityEngine;

public class Player : MonoBehaviour
{
    float currentHealth = 50f;
    float maxHealth = 100f;

    public AudioSource winSound;

    void Update()
    {
        // Y-Position in Weltkoordinaten fixieren
        /*
        Vector3 worldPos = transform.position;
        if (worldPos.y != 0f)
        {
            transform.position = new Vector3(worldPos.x, 0f, worldPos.z);
        }
        */
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Debug.Log("Lost  :( ");
            // TODO: Game Over Logic
        }
    }

    public void AddHealth(float hp)
    {
        if (hp < maxHealth)
        {
            currentHealth += hp;
        }
        if (currentHealth >= maxHealth)
        {
            winSound.Play();
            Debug.Log("Won  :) ");
            // TODO: Win Logic
        }
    }
}
