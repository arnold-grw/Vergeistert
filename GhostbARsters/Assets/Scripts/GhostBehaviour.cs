using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public Transform target; 
    public float maxSpeed = 1f;
    //public float brakeDistance = 3f;
    public float maxBackoff = 5f;
    public float minBackoff = 0.5f;
    private float currentBackoffDistance = 0.5f;
    private float currentSpeed = 0f;
    private float velocity = 0f;

    void Start()
    {
        currentBackoffDistance = minBackoff;
    }

    void Update()
    {
        Vector3 diff = target.position - transform.position;
        diff.y = 0; // Nur horizontale Bewegung

        // Drehe den Geist zum Spieler hin
        if (diff != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(diff);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }

        float distance = diff.magnitude;
        float targetSpeed = 0f;

        // Bestimme Zielgeschwindigkeit
        if (distance > currentBackoffDistance)
        {
            targetSpeed = maxSpeed;
        }
        if (distance < currentBackoffDistance)
        {
            targetSpeed = -maxSpeed/2;
        }

        // Smoothes Anpassen der Geschwindigkeit
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 2f * Time.deltaTime);
        //currentSpeed = targetSpeed;
        // Bewegung
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ConeMesh") // Oder vergleiche mit Tag etc.
        {
            currentBackoffDistance = maxBackoff;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ConeMesh") // Oder vergleiche mit Tag etc.
        {
            currentBackoffDistance = minBackoff;
        }
    }
}
