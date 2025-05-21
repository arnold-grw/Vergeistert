using UnityEngine;

public class Player : MonoBehaviour
{
    float Health = 100f;
    void Start()
    {

    }
    
    void Update()
    {

    }

    void takeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0f)
        {
            //Game Over :(
        }
    }
}
