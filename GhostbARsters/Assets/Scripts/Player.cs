using UnityEngine;

public class Player : MonoBehaviour
{
    float Health = 100f;

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
        Health -= damage;
        if (Health <= 0f)
        {
            Debug.Log("Game Over :( ");
            // TODO: Game Over Logik
        }
    }
}
