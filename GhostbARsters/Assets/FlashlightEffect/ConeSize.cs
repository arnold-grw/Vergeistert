using UnityEngine;

public class ConeSize : MonoBehaviour
{
    [Header("Einstellungen")]
    public float minDistance = 0f;
    public float maxDistance = 8f;
    public float smoothSpeed = 5f; // Glättungsgeschwindigkeit
    public LayerMask obstacleLayers;
    public float scaleFactor = 1.5f;

    private float targetScale;

    void Start()
    {
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.up;

        float targetDistance = maxDistance;

        // Raycast ausführen
        Debug.DrawRay(origin, direction * maxDistance, Color.red);
        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, obstacleLayers))
        {
            // Zielentfernung innerhalb min/max clampen
            targetDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }

        targetScale = Mathf.Lerp(targetScale, targetDistance, Time.deltaTime * smoothSpeed);
        transform.localScale = new Vector3(targetScale*scaleFactor, targetScale*scaleFactor, targetScale*scaleFactor);
    }
}
