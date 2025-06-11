using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : MonoBehaviour
{
    public Transform target; // Spieler oder Zielobjekt
    public static List<Ghost> allGhosts = new List<Ghost>();
    public GhostRenderer ghostRenderer;

    public float health = 10f;

    public float maxSpeed =0.5f;
    public float maxBackoff = 5f;
    public float minBackoff = 0.5f;

    private float currentBackoffDistance = 0.5f;
    private float currentSpeed = 0f;

    void Start()
    {
        currentBackoffDistance = minBackoff;
        allGhosts.Add(this);
    }

    public void setTarget(Transform t)
    {
        this.target = t;
    }

    void OnDestroy()
    {
        allGhosts.Remove(this);
    }

    void Update()
    {
        if (target == null) return;

        // Richtung und Abstand zum Ziel
        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0;
        float distanceToTarget = toTarget.magnitude;
        Vector3 directionToTarget = toTarget.normalized;

        // Zielgeschwindigkeit
        float targetSpeed = (distanceToTarget > currentBackoffDistance) ? maxSpeed : maxSpeed/10;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 2f * Time.deltaTime);

        // Bewegung: weg oder zum Ziel
        Vector3 baseMove = (currentSpeed >= 0) ? directionToTarget : -directionToTarget;

        // Abstoßungskraft
        Vector3 separationForce = Vector3.zero;
        float avoidanceRadius = 1.0f;
        float totalWeight = 0f;

        foreach (Ghost other in allGhosts)
        {
            if (other != this)
            {
                Vector3 offset = transform.position - other.transform.position;
                offset.y = 0;
                float dist = offset.magnitude;

                if (dist < avoidanceRadius && dist > 0.01f)
                {
                    float weight = (avoidanceRadius - dist) / avoidanceRadius;
                    separationForce += offset.normalized * weight;
                    totalWeight += weight;
                }
            }
        }

        // Durchschnittliche Abstoßung
        if (totalWeight > 0)
        {
            separationForce /= totalWeight;
        }

        // Kombinierte Bewegungsrichtung
        Vector3 moveDirection = baseMove + separationForce * 1.5f;
        moveDirection.y = 0;

        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();
            transform.position += moveDirection * Mathf.Abs(currentSpeed) * Time.deltaTime;
        }

        // Blickrichtung (nur zum Spieler, nicht durch Abstoßung gestört)
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ConeMesh") // oder per Tag prüfen
        {
            currentBackoffDistance = maxBackoff;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ConeMesh")
        {
            currentBackoffDistance = minBackoff;
        }
    }

    public void TakeDamage(float damage){
        this.health -= damage;
        if (health <= 0)
        {
            StartCoroutine(DestroyGhost());
        }
    }

    IEnumerator DestroyGhost()
    {
        float duration = 1f; // Fade-Zeit
        ghostRenderer.Dissolve(duration);
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
}
